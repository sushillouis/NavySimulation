using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(followStep != CommandSteps.finished)
        {
            SelectingFollow(followEnt);
        }
        else if (VRControlMgr.inst.rightGripPress.action.IsPressed())
        {
            if (VRControlMgr.inst.rightTriggerPress.action.WasPressedThisFrame())
            {
                if(Physics.OverlapSphere(rightRay.GetPosition(1), 50) != null)
                {
                    Vector3 pos = rightRay.GetPosition(1);
                    pos.y = 0;
                    Entity381 ent = AIMgr.inst.FindClosestEntInRadius(pos, AIMgr.inst.rClickRadiusSq);
                    if (ent == null)
                    {
                        AIMgr.inst.HandleMove(SelectionMgr.inst.selectedEntities, pos);
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
            for(int i = 0; i < followSelectLines.Count; i++)
            {
                LineRenderer l = followSelectLines[i];
                l.SetPosition(0, SelectionMgr.inst.selectedEntities[i].position);
                l.SetPosition(1, rightRay.GetPosition(1));
                l.SetPosition(2, target.position);
            }
            if (!VRControlMgr.inst.rightTriggerPress.action.IsPressed())
            {
                AIMgr.inst.HandleFollow(SelectionMgr.inst.selectedEntities, target, rightRay.GetPosition(1) - target.position);
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

    }
}
