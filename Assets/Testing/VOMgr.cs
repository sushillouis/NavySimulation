using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VO : MonoBehaviour
{
    public float theta;
    public float delta;
    public float alpha;

    public Entity381 ownship;
    public Entity381 target;

    public LineRenderer posDiff;
    public LineRenderer plusDelta;
    public LineRenderer minusDelta;

    public VO(Entity381 ownship, Entity381 target)
    {
        this.ownship = ownship;
        this.target = target;
    }

    public void CalcVO()
    {
        Potential p = DistanceMgr.inst.GetPotential(ownship, target);

        float relPosAngle = Mathf.Atan2(p.diff.x, p.diff.z) * Mathf.Rad2Deg; //theta
        float maxVOAngle = Mathf.Asin(550f / p.diff.magnitude) * Mathf.Rad2Deg; //delta
        Vector3 relVelOwn = -p.relativeVelocity;
        float relVelAngle = Mathf.Atan2(relVelOwn.x, relVelOwn.z) * Mathf.Rad2Deg; //alpha

        theta = relPosAngle;
        delta = maxVOAngle;
        alpha = relVelAngle;

    }

    public void DrawVO()
    {
        CalcVO();

        posDiff = LineMgr.inst.CreateVOLine(ownship.position, target.position);
        posDiff.gameObject.SetActive(true);

        float plusDeltaAngle = Utils.Degrees360(theta + delta);
        float minusDeltaAngle = Utils.Degrees360(theta - delta);

        Debug.Log(theta + "\n" + plusDeltaAngle + "\n" + minusDeltaAngle);

        Vector3 plusDeltaDirec = new Vector3(Mathf.Sin(plusDeltaAngle * Mathf.Deg2Rad), 0, Mathf.Cos(plusDeltaAngle * Mathf.Deg2Rad)).normalized;
        Vector3 minusDeltaDirect = new Vector3(Mathf.Sin(minusDeltaAngle * Mathf.Deg2Rad), 0, Mathf.Cos(minusDeltaAngle * Mathf.Deg2Rad)).normalized;

        Vector3 plusDeltaEndpoint = ownship.position + (plusDeltaDirec * 500);
        Vector3 minusDeltaEndpoint = ownship.position + (minusDeltaDirect * 500);

        posDiff = LineMgr.inst.CreateVOLine(ownship.position, target.position);
        posDiff.gameObject.SetActive(true);

        plusDelta = LineMgr.inst.CreateVOLine(ownship.position, plusDeltaEndpoint);
        plusDelta.gameObject.SetActive(true);
        minusDelta = LineMgr.inst.CreateVOLine(ownship.position, minusDeltaEndpoint);
        minusDelta.gameObject.SetActive(true);

    }
}

public class VOMgr : MonoBehaviour
{
    public Entity381 ownship;
    public Entity381 target;

    public VO test;

    public static VOMgr inst;
    
    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsGiveWay(Entity381 ownship, Entity381 target)
    {
        float rhown = DistanceMgr.inst.GetPotential(ownship, target).relativeBearingDegrees;
        float rht = DistanceMgr.inst.GetPotential(target, ownship).relativeBearingDegrees;

        if (rhown < 112.5f || rhown > 350)
            return true;
        else if(Utils.AngleDiffPosNeg(rhown, 112.5f) < Utils.AngleDiffPosNeg(rht, 112.5f)) //not sure if this is right
            return true;
        else
            return false;
    }

    public List<Entity381> DetectCollisions(Entity381 ownship, List<Entity381> entities)
    {
        List<Entity381> output = new List<Entity381>();

        foreach (Entity381 target in entities)
        {
            Potential p = DistanceMgr.inst.GetPotential(ownship, target);

            VO velObs = new VO(ownship, target);
            velObs.CalcVO();

            float angleDiff = (velObs.theta - velObs.alpha + 180 + 360) % 360 - 180;

            if (p.distance < 550 || (angleDiff <= velObs.delta && angleDiff >= -velObs.delta))
                output.Add(target);
        }
        return output;
    }  
}
