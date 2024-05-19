using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public class VO
{
    public float theta;
    public float delta;
    public float alpha;

    public Entity381 ownship;
    public Entity381 target;

    public LineRenderer plusDeltaLine;
    public LineRenderer minusDeltaLine;
    public LineRenderer alphaLine;
    public LineRenderer radius;

    public bool showingVOs;

    public VO(Entity381 ownship, Entity381 target)
    {
        this.ownship = ownship;
        this.target = target;
        showingVOs = false;
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

    public float CalcAlpha(Vector3 velocity)
    {
        Vector3 relVelOwn = velocity - target.velocity;
        float relVelAngle = Mathf.Atan2(relVelOwn.x, relVelOwn.z) * Mathf.Rad2Deg; //alpha

        return relVelAngle;
    }

    public void InitializeVODrawing()
    {
        CalcVO();

        plusDeltaLine = LineMgr.inst.CreateVOLine(ownship.position, ownship.position);
        plusDeltaLine.gameObject.SetActive(true);

        minusDeltaLine = LineMgr.inst.CreateVOLine(ownship.position, ownship.position);
        minusDeltaLine.gameObject.SetActive(true);

        alphaLine = LineMgr.inst.CreateVOLine(ownship.position, ownship.position);
        alphaLine.gameObject.SetActive(true);

        showingVOs = true;
    }

    public void DrawVO()
    {
        CalcVO();

        float plusDeltaAngle = Utils.Degrees360(theta + delta);
        float minusDeltaAngle = Utils.Degrees360(theta - delta);

        Vector3 plusDeltaDirec = new Vector3(Mathf.Sin(plusDeltaAngle * Mathf.Deg2Rad), 0, Mathf.Cos(plusDeltaAngle * Mathf.Deg2Rad)).normalized;
        Vector3 minusDeltaDirec = new Vector3(Mathf.Sin(minusDeltaAngle * Mathf.Deg2Rad), 0, Mathf.Cos(minusDeltaAngle * Mathf.Deg2Rad)).normalized;
        Vector3 alphaDirec = new Vector3(Mathf.Sin(alpha * Mathf.Deg2Rad), 0, Mathf.Cos(alpha * Mathf.Deg2Rad)).normalized;

        Vector3 plusDeltaEndpoint = ownship.position + (plusDeltaDirec * (ownship.position - target.position).magnitude);
        Vector3 minusDeltaEndpoint = ownship.position + (minusDeltaDirec * (ownship.position - target.position).magnitude);
        Vector3 alphaEndpoint = ownship.position + (alphaDirec * 500);

        plusDeltaLine.SetPosition(0, ownship.position);
        plusDeltaLine.SetPosition(1, plusDeltaEndpoint);

        minusDeltaLine.SetPosition(0, ownship.position);
        minusDeltaLine.SetPosition(1, minusDeltaEndpoint);

        alphaLine.SetPosition(0, ownship.position);
        alphaLine.SetPosition(1, alphaEndpoint);
    }

    public void DrawRadius()
    {
        radius = LineMgr.inst.CreateVOLine(target.position, target.position + (new Vector3(1,0,1).normalized * 550));
        radius.gameObject.SetActive(true);
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
        if(test != null && test.showingVOs)
        {
            test.DrawVO();
        }
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

    public List<Entity381> DetectCollisions(Vector3 velocity, Entity381 ownship, List<Entity381> entities)
    {
        List<Entity381> output = new List<Entity381>();

        foreach (Entity381 target in entities)
        {
            Potential p = DistanceMgr.inst.GetPotential(ownship, target);

            VO velObs = new VO(ownship, target);
            velObs.CalcVO();
            float alpha = velObs.CalcAlpha(velocity);

            float angleDiff = (velObs.theta - alpha + 180 + 360) % 360 - 180;

            if (p.distance < 550 || (angleDiff <= velObs.delta && angleDiff >= -velObs.delta))
                output.Add(target);
        }
        return output;
    }  

    public DHDS AvoidCollisions(Entity381 ownship, List<Entity381> entites)
    {
        List<Entity381> vesselsToGWTo = new List<Entity381>();

        foreach (Entity381 target in entites)
        {
            if(IsGiveWay(ownship, target))
                vesselsToGWTo.Add(target);
        }

        List<Entity381> riskObstacles = DetectCollisions(ownship.velocity, ownship, vesselsToGWTo);

        Entity381 priorityEnt = null;
        float minTCPA = Mathf.Infinity;

        foreach(Entity381 target in riskObstacles)
        {
            Potential p = DistanceMgr.inst.GetPotential(ownship, target);

            if(p.cpaInfo.time < minTCPA && p.cpaInfo.time < 200)
            {
                priorityEnt = target;
                minTCPA = p.cpaInfo.time;
            }
        }

        if(priorityEnt != null)
        {
            float bestAngle = ownship.heading;
            float bestSpeed = 0;
            float bestDCPA = Mathf.Infinity;
            float bestTCPA = 0;


            for(float angle = 0; angle <= 120; angle += 5)
            {
                for(float speed = 1; speed >= 0; speed -= 0.25f)
                {
                    float newHeading = Utils.Degrees360(ownship.heading + angle);
                    Vector3 newVelocity = ownship.maxSpeed * speed * new Vector3(Mathf.Sin(newHeading * Mathf.Deg2Rad), 0, Mathf.Cos(newHeading * Mathf.Deg2Rad));
                    List<Entity381> newRiskObstacles = DetectCollisions(newVelocity, ownship, entites);

                    if(newRiskObstacles.Count == 0 && speed >= bestSpeed)
                    {
                        Vector3 prioRelPos = priorityEnt.position - ownship.position;
                        Vector3 prioRelVel = priorityEnt.velocity - newVelocity;
                        float t = Mathf.Acos(Vector3.Dot(-prioRelPos, prioRelVel) / (prioRelPos.magnitude * prioRelVel.magnitude)); //i'm not sure really what this is supposed to represent
                        float DCPA = prioRelPos.magnitude * Mathf.Sin(t);
                        float TCPA = prioRelPos.magnitude * Mathf.Cos(t) / prioRelVel.magnitude;

                        if(DCPA < bestDCPA && DCPA > 550 && TCPA > 0)
                        {
                            bestAngle = newHeading;
                            bestSpeed = speed;
                            bestDCPA = DCPA;
                            bestTCPA = TCPA;
                        }
                    }
                }
            }

            return new DHDS(bestAngle, ownship.maxSpeed * bestSpeed);
        }
        else return null;
    }
}
