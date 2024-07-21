using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum CommandSteps
{
    started,
    selecting,
    finished
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
        if(followStep == CommandSteps.started)
        {
            followSelectLines = new List<LineRenderer>();
            foreach(Entity381 ent in SelectionMgr.inst.selectedEntities)
            {
                followSelectLines.Add(LineMgr.inst.CreateFollowLine(ent.position, rightRay.GetPosition(1), target.position));
            }
            followStep = CommandSteps.selecting;
        }
        if(followStep == CommandSteps.selecting)
        {
            if(rightRay.GetPosition(1).y < Utils.EPSILON)
            {
                for (int i = 0; i < followSelectLines.Count; i++)
                {
                    LineRenderer l = followSelectLines[i];
                    l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, rightRay.GetPosition(1));
                    l.SetPosition(2, target.position);
                }
            }
            else
            {
                for (int i = 0; i < followSelectLines.Count; i++)
                {
                    LineRenderer l = followSelectLines[i];
                    l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(2, SelectionMgr.inst.selectedEntities[i].position);
                }
            }
            if (!VRControlMgr.inst.rightTriggerPress.action.IsPressed())
            {
                if(rightRay.GetPosition(1).y < Utils.EPSILON)
                {
                    Vector3 offset = target.transform.InverseTransformPoint(rightRay.GetPosition(1));
                    AIMgr.inst.HandleFollow(SelectionMgr.inst.selectedEntities, target, offset);
                }
                followStep = CommandSteps.finished;
                for(int i = followSelectLines.Count - 1; i > -1; i--)
                {
                    LineMgr.inst.DestroyLR(followSelectLines[i]);
                }
            }

        }
    }

    void SelectingMove()
    {
        if (moveStep == CommandSteps.started)
        {
            moveSelectLines = new List<LineRenderer>();
            foreach (Entity381 ent in SelectionMgr.inst.selectedEntities)
            {
                moveSelectLines.Add(LineMgr.inst.CreateMoveLine(ent.position, rightRay.GetPosition(1)));
            }
            moveStep = CommandSteps.selecting;
        }
        if (moveStep == CommandSteps.selecting)
        {
            if (rightRay.GetPosition(1).y < Utils.EPSILON)
            {
                for (int i = 0; i < moveSelectLines.Count; i++)
                {
                    LineRenderer l = moveSelectLines[i];
                    l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, rightRay.GetPosition(1));
                }
            }
            else
            {
                for (int i = 0; i < moveSelectLines.Count; i++)
                {
                    LineRenderer l = moveSelectLines[i];
                    l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                    l.SetPosition(1, SelectionMgr.inst.selectedEntities[i].position);
                }
            }
            if (!VRControlMgr.inst.rightTriggerPress.action.IsPressed())
            {
                if (rightRay.GetPosition(1).y < Utils.EPSILON)
                {
                    AIMgr.inst.HandleMove(SelectionMgr.inst.selectedEntities, rightRay.GetPosition(1));
                }
                moveStep = CommandSteps.finished;
                for (int i = moveSelectLines.Count - 1; i > -1; i--)
                {
                    LineMgr.inst.DestroyLR(moveSelectLines[i]);
                }
            }

        }
    }
}
