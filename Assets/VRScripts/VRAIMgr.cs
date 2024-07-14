using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAIMgr : MonoBehaviour
{
    public LineRenderer rightRay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (VRControlMgr.inst.rightGripPress.action.IsPressed())
        {
            if (VRControlMgr.inst.rightTriggerPress.action.IsPressed())
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
                            AIMgr.inst.HandleFollow(SelectionMgr.inst.selectedEntities, ent);
                    }
                }

            }
        }
    }
}
