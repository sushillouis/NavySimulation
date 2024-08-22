using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum CommandSteps
{
    started,
    selecting,
    finished,
    selectingTarget
}

public class VRAIMgr : MonoBehaviour
{
    public LineRenderer rightRay;
    public static VRAIMgr inst;

    Entity381 followEnt;
    List<LineRenderer> followSelectLines;
    public CommandSteps followStep = CommandSteps.finished;

    List<LineRenderer> moveSelectLines;
    public CommandSteps moveStep = CommandSteps.finished;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (VRControlMgr.inst.XButtonPress.action.IsPressed())
            AIMgr.addDown = true;
        else
            AIMgr.addDown = false;

        if (followStep != CommandSteps.finished)
            SelectingFollow(followEnt);
        else if (moveStep != CommandSteps.finished)
            SelectingMove();
        else if (VRControlMgr.inst.rightGripPress.action.IsPressed())
        {
            if (VRControlMgr.inst.rightTriggerPress.action.WasPressedThisFrame())
            {
                if (Physics.OverlapSphere(rightRay.GetPosition(1), 50) != null)
                {
                    Vector3 pos = rightRay.GetPosition(1);
                    pos.y = 0;
                    Entity381 ent = AIMgr.inst.FindClosestEntInRadius(pos, AIMgr.inst.rClickRadiusSq);
                    if (ent == null)
                    {
                        moveStep = CommandSteps.started;
                    }
                    else
                    {
                        if (VRControlMgr.inst.AButtonPress.action.IsPressed())
                            AIMgr.inst.HandleIntercept(SelectionMgr.inst.selectedEntities, ent);
                        else
                        {
                            followEnt = ent;
                            followStep = CommandSteps.started;
                        }
                    }
                }
            }
        }
    }

    void SelectingFollow(Entity381 target)
    {
        //Initializes selecting lines
        if (followStep == CommandSteps.started)
        {
            followSelectLines = new List<LineRenderer>();
            foreach(Entity381 ent in SelectionMgr.inst.selectedEntities)
            {
                followSelectLines.Add(LineMgr.inst.CreateFollowLine(ent.position, rightRay.GetPosition(1), target.position));
            }
            followStep = CommandSteps.selecting;
        }

        //After initializing
        if (followStep == CommandSteps.selecting)
        {
            if(rightRay.GetPosition(1).y < Utils.EPSILON) //Checks if the ray pointer is hitting the ocean
            {
                for (int i = 0; i < followSelectLines.Count; i++)
                {
                    //add command logic
                    LineRenderer l = followSelectLines[i];
                    if (AIMgr.addDown || (CommandsMgr.inst.startCommandCondition != CommandCondition.NoCondition && CommandsMgr.inst.insertWhenAdded)) //Checks whether or not the command is an add or a set
                    {
                        UnitAI uai = SelectionMgr.inst.selectedEntities[i].GetComponent<UnitAI>();
                        int lastCommandIndex = uai.commands.Count - 1;

                        if (lastCommandIndex > -1)
                            l.SetPosition(0, uai.commands[lastCommandIndex].movePosition);
                        else //if there are no prexisting commands, start the line at ownship
                            l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    }
                    else //set command logic
                        l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, rightRay.GetPosition(1));
                    l.SetPosition(2, target.position);
                }
            }
            else //if the ray pointer isn't hitting the ocean, don't show the lines
            {
                for (int i = 0; i < followSelectLines.Count; i++)
                {
                    LineRenderer l = followSelectLines[i];
                    l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(2, SelectionMgr.inst.selectedEntities[i].position);
                }
            }

            //Once the trigger is released, finalize the command
            if (!VRControlMgr.inst.rightTriggerPress.action.IsPressed())
            {
                if(rightRay.GetPosition(1).y < Utils.EPSILON) //Checks if the ray pointer is hitting the ocean, only set the command if it is
                {
                    Vector3 offset = target.transform.InverseTransformPoint(rightRay.GetPosition(1));
                    AIMgr.inst.HandleFollow(SelectionMgr.inst.selectedEntities, target, offset);
                }
                followStep = CommandSteps.finished;
                for(int i = followSelectLines.Count - 1; i > -1; i--) //destroys the selection lines
                {
                    LineMgr.inst.DestroyLR(followSelectLines[i]);
                }
            }

        }
    }

    void SelectingMove()
    {
        //Initializes selecting lines
        if (moveStep == CommandSteps.started)
        {
            moveSelectLines = new List<LineRenderer>();
            foreach (Entity381 ent in SelectionMgr.inst.selectedEntities)
            {
                moveSelectLines.Add(LineMgr.inst.CreateMoveLine(ent.position, rightRay.GetPosition(1)));
            }
            moveStep = CommandSteps.selecting;
        }

        //After initializing
        if (moveStep == CommandSteps.selecting)
        {
            if (rightRay.GetPosition(1).y < Utils.EPSILON) //Checks if the ray pointer hit is hitting the ocean
            {
                for (int i = 0; i < moveSelectLines.Count; i++)
                {
                    LineRenderer l = moveSelectLines[i];
                    if (AIMgr.addDown || (CommandsMgr.inst.startCommandCondition != CommandCondition.NoCondition && CommandsMgr.inst.insertWhenAdded)) //Checks whether or not the command is an add or a set
                    {
                        UnitAI uai = SelectionMgr.inst.selectedEntities[i].GetComponent<UnitAI>();
                        int lastCommandIndex = uai.commands.Count - 1;

                        if (lastCommandIndex > -1)
                            l.SetPosition(0, uai.commands[lastCommandIndex].movePosition);
                        else //if there are no prexisting commands, start the line at ownship
                            l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    }
                    else //set command logic
                        l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, rightRay.GetPosition(1));
                }
            }
            else //if the ray pointer hit isn't hitting the ocean, don't show the lines
            {
                for (int i = 0; i < moveSelectLines.Count; i++)
                {
                    LineRenderer l = moveSelectLines[i];
                    l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, SelectionMgr.inst.selectedEntities[i].position);
                }
            }

            //Once the trigger is released, finalize the command
            if (!VRControlMgr.inst.rightTriggerPress.action.IsPressed())
            {
                if (rightRay.GetPosition(1).y < Utils.EPSILON) //Checks if the raycast hit is hitting the ocean, only set the command if it is
                {
                    AIMgr.inst.HandleMove(SelectionMgr.inst.selectedEntities, rightRay.GetPosition(1));
                }
                moveStep = CommandSteps.finished;
                for (int i = moveSelectLines.Count - 1; i > -1; i--) //destroys the selection lines
                {
                    LineMgr.inst.DestroyLR(moveSelectLines[i]);
                }
            }

        }
    }
}
