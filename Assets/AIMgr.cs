using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementType
{
    Regular,
    PotentialFields,
    VelocityObstacles
}

public class AIMgr : MonoBehaviour
{
    public static AIMgr inst;
    private GameInputs input;
    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << 9;// LayerMask.GetMask("Water");
        input = new GameInputs();
        input.Enable();
        input.Entities.Move.started += OnMoveStarted;
        input.Entities.Move.performed += OnMovePerformed;
        input.Entities.Move.canceled += OnMoveCanceled;
        input.Entities.Intercept.performed += OnInterceptPerformed;
        input.Entities.Intercept.canceled += OnInterceptCanceled;
        input.Entities.ClearSelection.performed += OnClearSelectionPerformed;
        input.Entities.ClearSelection.canceled += OnClearSelectionCanceled;
    }

    public MovementType movementType = MovementType.Regular;

    [Header("VO Parameters")]
    public float collisionRadius = 550;
    public float tcpaLimit = 200;
    public bool useSetCollisionRadius = false;

    [Header("Potential Parameters")]
    public float potentialDistanceThreshold = 1000;
    public float attractionCoefficient = 500;
    public float attractiveExponent = -1;
    public float repulsiveCoefficient = 60000;
    public float repulsiveExponent = -2.0f;

    [Header("Other Parameters")]
    public int layerMask;
    public RaycastHit hit;
    

    // Update is called once per frame
    void Update()
    {
        if (followStep != CommandSteps.finished)
            SelectingFollow(followEnt);
        else if (moveStep != CommandSteps.finished)
            SelectingMove();
        else if (moveClicked)
        {
            moveClicked = false;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, layerMask))
            {
                //Debug.DrawLine(Camera.main.transform.position, hit.point, Color.yellow, 2); //for debugging
                Vector3 pos = hit.point;
                pos.y = 0;
                Entity381 ent = FindClosestEntInRadius(pos, rClickRadiusSq);
                if (ent == null)
                    moveStep = CommandSteps.started;
                else
                {
                    if (interceptDown)
                        HandleIntercept(SelectionMgr.inst.selectedEntities, ent);
                    else
                    {
                        followEnt = ent;
                        followStep = CommandSteps.started;
                    }
                }
            }
            else
            {
                //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 1000, Color.white, 2);
            }
        }
    }

    Entity381 followEnt;
    List<LineRenderer> followSelectLines;
    CommandSteps followStep = CommandSteps.finished;

    void SelectingFollow(Entity381 target)
    {
        //Update hit
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, layerMask);

        //Initializes selecting lines
        if (followStep == CommandSteps.started)
        {
            followSelectLines = new List<LineRenderer>();
            foreach (Entity381 ent in SelectionMgr.inst.selectedEntities) //iterates through selected ents and creates a selecting line for each
            {
                followSelectLines.Add(LineMgr.inst.CreateFollowLine(ent.position, hit.point, target.position));
            }
            followStep = CommandSteps.selecting;
        }

        //After initializing
        if (followStep == CommandSteps.selecting)
        {
            if (hit.point.y < 0) //Checks if the raycast hit is hitting the ocean
            {
                for (int i = 0; i < followSelectLines.Count; i++)
                {
                    LineRenderer l = followSelectLines[i];
                    if (addDown) //Checks whether or not the command is an add or a set
                    {
                        //add command logic
                        UnitAI uai = SelectionMgr.inst.selectedEntities[i].GetComponent<UnitAI>();
                        int lastCommandIndex = uai.commands.Count - 1;

                        if (lastCommandIndex > -1)
                            l.SetPosition(0, uai.commands[lastCommandIndex].movePosition);
                        else //if there are no prexisting commands, start the line at ownship
                            l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    }
                    else //set command logic
                        l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, hit.point);
                    l.SetPosition(2, target.position);
                }
            }
            else //if the raycast hit isn't hitting the ocean, don't show the lines
            {
                for (int i = 0; i < followSelectLines.Count; i++)
                {
                    LineRenderer l = followSelectLines[i];
                    l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(2, SelectionMgr.inst.selectedEntities[i].position);
                }
            }

            //Once the mouse is released, finalize the command
            if (!moveDown) 
            {
                if(hit.point.y < 0) //Checks if the raycast hit is hitting the ocean, only set the command if it is
                {
                    Vector3 offset = target.transform.InverseTransformPoint(hit.point);
                    AIMgr.inst.HandleFollow(SelectionMgr.inst.selectedEntities, target, offset);
                }

                followStep = CommandSteps.finished;
                for (int i = followSelectLines.Count - 1; i > -1; i--) //destroys the selection lines
                {
                    LineMgr.inst.DestroyLR(followSelectLines[i]);
                }
            }

        }
    }

    List<LineRenderer> moveSelectLines;
    public CommandSteps moveStep = CommandSteps.finished;

    void SelectingMove()
    {
        //Update hit
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, layerMask);

        //Initializes selecting lines
        if (moveStep == CommandSteps.started)
        {
            moveSelectLines = new List<LineRenderer>();
            foreach (Entity381 ent in SelectionMgr.inst.selectedEntities) //iterates through selected ents and creates a selecting line for each
            {
                moveSelectLines.Add(LineMgr.inst.CreateMoveLine(ent.position, hit.point));
            }
            moveStep = CommandSteps.selecting;
        }

        //After initializing
        if (moveStep == CommandSteps.selecting)
        {
            if (hit.point.y < 0) //Checks if the raycast hit is hitting the ocean
            {
                for (int i = 0; i < moveSelectLines.Count; i++)
                {
                    LineRenderer l = moveSelectLines[i];
                    if (addDown) //Checks whether or not the command is an add or a set
                    {
                        //add command logic
                        UnitAI uai = SelectionMgr.inst.selectedEntities[i].GetComponent<UnitAI>();
                        int lastCommandIndex = uai.commands.Count - 1;
                        
                        if (lastCommandIndex > -1)
                            l.SetPosition(0, uai.commands[lastCommandIndex].movePosition);
                        else //if there are no prexisting commands, start the line at ownship
                            l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    }
                    else //set command logic
                        l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, hit.point);
                }
            }
            else //if the raycast hit isn't hitting the ocean, don't show the lines
            {
                for (int i = 0; i < moveSelectLines.Count; i++)
                {
                    LineRenderer l = moveSelectLines[i];
                    l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, SelectionMgr.inst.selectedEntities[i].position);
                }
            }

            //Once the mouse is released, finalize the command
            if (!moveDown) 
            {
                if (hit.point.y < 0) //Checks if the raycast hit is hitting the ocean, only set the command if it is
                    AIMgr.inst.HandleMove(SelectionMgr.inst.selectedEntities, hit.point);

                moveStep = CommandSteps.finished;
                for (int i = moveSelectLines.Count - 1; i > -1; i--) //destroys the selection lines
                {
                    LineMgr.inst.DestroyLR(moveSelectLines[i]); 
                }
            }

        }
    }

    public void HandleMove(List<Entity381> entities, Vector3 point)
    {
        foreach (Entity381 entity in entities) {
            Move m = new Move(entity, point);
            m.condition = CommandsMgr.inst.moveCommandCondition;
            m.distanceThreshold = CommandsMgr.inst.moveDistanceThreshold;
            m.timeThreshold = CommandsMgr.inst.moveTimeThreshold;
            m.conditionEntity = CommandsMgr.inst.moveEntity;
            m.conditionEntityType = CommandsMgr.inst.moveEntityType;

            UnitAI uai = entity.GetComponent<UnitAI>();
            AddOrSet(m, uai);
        }
    }

    void AddOrSet(Command c, UnitAI uai)
    {
        if (c.startCommand)
            uai.HandleStartCommand(c);
        if (addDown)
            uai.AddCommand(c);
        else
            uai.SetCommand(c);
    }



    public void HandleFollow(List<Entity381> entities, Entity381 ent, Vector3 offset)
    {
        foreach (Entity381 entity in SelectionMgr.inst.selectedEntities) {
            Follow f = new Follow(entity, ent, offset);
            f.condition = CommandsMgr.inst.followCommandCondition;
            f.distanceThreshold = CommandsMgr.inst.followDistanceThreshold;
            f.timeThreshold = CommandsMgr.inst.followTimeThreshold;
            f.conditionEntity = CommandsMgr.inst.followEntity;
            f.conditionEntityType = CommandsMgr.inst.followEntityType;
            f.fromCaughtUp = CommandsMgr.inst.fromFollow;

            UnitAI uai = entity.GetComponent<UnitAI>();
            AddOrSet(f, uai);
        }
    }

    public void HandleIntercept(List<Entity381> entities, Entity381 ent)
    {
        foreach (Entity381 entity in SelectionMgr.inst.selectedEntities) {
            Intercept intercept = new Intercept(entity, ent);
            UnitAI uai = entity.GetComponent<UnitAI>();
            AddOrSet(intercept, uai);
        }

    }

    public float rClickRadiusSq = 10000;
    public Entity381 FindClosestEntInRadius(Vector3 point, float rsq)
    {
        Entity381 minEnt = null;
        float min = float.MaxValue;
        foreach (Entity381 ent in EntityMgr.inst.entities) {
            float distanceSq = (ent.transform.position - point).sqrMagnitude;
            if (distanceSq < rsq) {
                if (distanceSq < min) {
                    minEnt = ent;
                    min = distanceSq;
                }
            }    
        }
        return minEnt;
    }

    bool interceptDown = false;
    private void OnInterceptPerformed(InputAction.CallbackContext context)
    {
        interceptDown = true;
    }

    private void OnInterceptCanceled(InputAction.CallbackContext context)
    {
        interceptDown = false;
    }

    public static bool addDown = false;
    private void OnClearSelectionPerformed(InputAction.CallbackContext context)
    {
        addDown = true;
    }

    private void OnClearSelectionCanceled(InputAction.CallbackContext context)
    {
        addDown = false;
    }

    bool moveClicked = false;
    bool moveDown = false;
    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        moveClicked = true;
    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveDown = true;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveDown = false;
    }
}
