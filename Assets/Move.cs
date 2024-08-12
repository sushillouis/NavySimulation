using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum CommandCondition
{
    NoCondition,
    InRangeOfASpecificEntity,
    InRangeOfTypeOfEntity,
    TimeFromCommandStart,
    DistanceTraveled,
    DistanceFromStart,
    TimeFromFollowing,
    DistanceFromFollowing
}

[System.Serializable]
public class Move : Command
{
    public Move(Entity381 ent, Vector3 pos) : base(ent)
    {
        movePosition = pos;
    }

    public LineRenderer potentialLine;
    public override void Init()
    {
        //Debug.Log("MoveInit:\tMoving to: " + movePosition);
        line = LineMgr.inst.CreateMoveLine(entity.position, movePosition);
        line.gameObject.SetActive(false);
        if(AIMgr.inst.movementType == MovementType.PotentialFields)
            potentialLine = LineMgr.inst.CreatePotentialLine(entity.position);
        line.gameObject.SetActive(true);
        startPosition = entity.position;
    }

    public override void Tick()
    {
        DHDS dhds;
        if (AIMgr.inst.movementType == MovementType.PotentialFields)
            dhds = ComputePotentialDHDS();
        else if (AIMgr.inst.movementType == MovementType.VelocityObstacles)
            dhds = ComputeVODHDS(entity, EntityMgr.inst.entities);
        else
            dhds = ComputeDHDS();

        entity.desiredHeading = dhds.dh;
        entity.desiredSpeed = dhds.ds;

        commandTime += Time.deltaTime;
        distanceTraveled += entity.speed * Time.deltaTime;

        line.SetPosition(1, movePosition);
    }

    Entity381 priorityEnt = null;

    public DHDS ComputeVODHDS(Entity381 ownship, List<Entity381> entities)
    {
        List<Entity381> riskObstacles = DetectRiskEntities(ownship.velocity, ownship, entities);

        float tcpaPrio = priorityEnt != null ? Utils.TCPA(entity, priorityEnt) : 0f;
        if (tcpaPrio < 0f)
            priorityEnt = null;

        foreach (Entity381 target in riskObstacles)
        {
            float TCPA = Utils.TCPA(ownship, target);

            if (TCPA >= 0 && TCPA < AIMgr.inst.tcpaLimit)
            {
                if (priorityEnt == null || TCPA < tcpaPrio)
                {
                    priorityEnt = target;
                    tcpaPrio = TCPA;
                }
            }
        }

        if (priorityEnt != null)
            return FindVODHDS(ownship, priorityEnt, entities);
        else
            return ComputeDHDS();
    }

    public List<Entity381> DetectRiskEntities(Vector3 velocity, Entity381 ownship, List<Entity381> entities)
    {
        List<Entity381> output = new List<Entity381>();

        foreach (Entity381 target in entities)
        {
            if (target == ownship) continue;

            float dist = Vector3.Distance(ownship.position, target.position);

            VO velObs = VOMgr.inst.GetVO(ownship, target);
            //VO velObs = new VO(ownship, target);
            //velObs.CalcVO();
            float alpha = velObs.CalcAlpha(velocity);

            if (dist < velObs.collisionRadius || (Utils.AngleBetween(alpha, velObs.minusDelta, velObs.plusDelta)))
            {
                if (velObs.giveWay)
                    output.Add(target);
            }
        }
        return output;
    }

    public DHDS FindVODHDS(Entity381 ownship, Entity381 priorityEnt, List<Entity381> entities)
    {
        float bestAngle = ownship.heading;
        float bestSpeed = 0;
        float bestDCPA = Mathf.Infinity;

        for (float angle = 0; angle <= 120; angle += 5)
        {
            for (float speed = 1; speed >= 0; speed -= 0.25f)
            {
                float newHeading = Utils.Degrees360(ownship.heading + angle);
                Vector3 newVelocity = ownship.maxSpeed * speed * new Vector3(Mathf.Sin(newHeading * Mathf.Deg2Rad), 0, Mathf.Cos(newHeading * Mathf.Deg2Rad));
                List<Entity381> newRiskObstacles = DetectRiskEntities(newVelocity, ownship, entities);

                if (newRiskObstacles.Count == 0 && speed >= bestSpeed)
                {
                    Vector3 prioRelPos = priorityEnt.position - ownship.position;
                    Vector3 prioRelVel = priorityEnt.velocity - newVelocity;
                    float t = Mathf.Acos(Vector3.Dot(-prioRelPos, prioRelVel) / (prioRelPos.magnitude * prioRelVel.magnitude));
                    float DCPA = prioRelPos.magnitude * Mathf.Sin(t);
                    float TCPA = prioRelPos.magnitude * Mathf.Cos(t) / prioRelVel.magnitude;

                    VO velObs = VOMgr.inst.GetVO(ownship, priorityEnt);

                    if (DCPA < bestDCPA && DCPA > velObs.collisionRadius && TCPA > 0)
                    {
                        bestAngle = newHeading;
                        bestSpeed = speed;
                        bestDCPA = DCPA;
                    }
                }
            }
        }

        return new DHDS(bestAngle, ownship.maxSpeed * bestSpeed);
    }

    public Vector3 diff = Vector3.positiveInfinity;
    public float dhRadians;
    public float dhDegrees;
    public DHDS ComputeDHDS()
    {
        diff = movePosition - entity.position;
        dhRadians = Mathf.Atan2(diff.x, diff.z);
        dhDegrees = Utils.Degrees360(Mathf.Rad2Deg * dhRadians);
        return new DHDS(dhDegrees, entity.maxSpeed);

    }

    public DHDS ComputePotentialDHDS()
    {
        Potential p;
        repulsivePotential = Vector3.one; repulsivePotential.y = 0;
        foreach (Entity381 ent in EntityMgr.inst.entities) {
            if (ent == entity) continue;
            p = DistanceMgr.inst.GetPotential(entity, ent);
            if (p.distance < AIMgr.inst.potentialDistanceThreshold) {
                repulsivePotential += p.direction * entity.mass *
                    AIMgr.inst.repulsiveCoefficient * Mathf.Pow(p.diff.magnitude, AIMgr.inst.repulsiveExponent);
                //repulsivePotential += p.diff;
            }
        }
        //repulsivePotential *= repulsiveCoefficient * Mathf.Pow(repulsivePotential.magnitude, repulsiveExponent);
        attractivePotential = movePosition - entity.position;
        Vector3 tmp = attractivePotential.normalized;
        attractivePotential = tmp * 
            AIMgr.inst.attractionCoefficient * Mathf.Pow(attractivePotential.magnitude, AIMgr.inst.attractiveExponent);
        potentialSum = attractivePotential - repulsivePotential;

        dh = Utils.Degrees360(Mathf.Rad2Deg * Mathf.Atan2(potentialSum.x, potentialSum.z));

        angleDiff = Utils.Degrees360(Utils.AngleDiffPosNeg(dh, entity.heading));
        cosValue = (Mathf.Cos(angleDiff * Mathf.Deg2Rad) + 1) / 2.0f; // makes it between 0 and 1
        ds = entity.maxSpeed * cosValue;

        return new DHDS(dh, ds);
    }
    public Vector3 attractivePotential = Vector3.zero;
    public Vector3 potentialSum = Vector3.zero;
    public Vector3 repulsivePotential = Vector3.zero;
    public float dh;
    public float angleDiff;
    public float cosValue;
    public float ds;

    bool CheckIfEntityTypeInRange(EntityType entityType, float range)
    {
        Collider[] colliders = Physics.OverlapSphere(entity.position, range);

        foreach(Collider collider in colliders)
        {
            Entity381 target = collider.transform.GetComponent<Entity381>();
            if (target != null && target != entity && target.entityType == entityType)
                return true;
        }

        return false;
    }

    public float doneDistanceSq = 1000;
    public override bool IsDone()
    {
        bool baseCondition = ((entity.position - movePosition).sqrMagnitude < doneDistanceSq);

        if (condition == CommandCondition.InRangeOfASpecificEntity)
            return baseCondition || ((entity.position - conditionEntity.position).sqrMagnitude < (distanceThreshold * distanceThreshold));
        else if (condition == CommandCondition.InRangeOfTypeOfEntity)
            return baseCondition || CheckIfEntityTypeInRange(conditionEntityType, distanceThreshold);
        else if (condition == CommandCondition.TimeFromCommandStart)
            return baseCondition || (commandTime > timeThreshold);
        else if (condition == CommandCondition.DistanceTraveled)
            return baseCondition || (distanceTraveled > distanceThreshold);
        else if (condition == CommandCondition.DistanceFromStart)
            return baseCondition || ((entity.position - startPosition).sqrMagnitude > distanceThreshold * distanceThreshold);
        else
            return baseCondition;
    }

    public override void Stop()
    {
        entity.desiredSpeed = 0;
        LineMgr.inst.DestroyLR(line);
        LineMgr.inst.DestroyLR(potentialLine);
    }
}
