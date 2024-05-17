using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VOMgr : MonoBehaviour
{
    public Entity381 ownship;
    public Entity381 target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isGiveWay(Entity381 ownship, Entity381 target)
    {
        float rhown = DistanceMgr.inst.GetPotential(ownship, target).relativeBearingDegrees;
        float rht = DistanceMgr.inst.GetPotential(target, ownship).relativeBearingDegrees;

        if (rhown < 112.5f || rhown > 350)
            return true;
        else if(Utils.AngleDiffPosNeg(rhown, 112.5f) > Utils.AngleDiffPosNeg(rht, 112.5f))
            return true;
        else
            return false;
    }
}
