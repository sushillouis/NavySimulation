using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Follow : Move
{
    public bool useRegular = false;

    public Entity381 targetEntity;
    public Vector3 relativeOffset;

    public Follow(Entity381 ent, Entity381 target, Vector3 delta): base(ent, target.transform.position)
    {
        targetEntity = target;
        relativeOffset = delta;
    }

    // Start is called before the first frame update
    public override void Init()
    {
        //Debug.Log("Follow:\t Following: " + targetEntity.gameObject.name);
        offset = targetEntity.transform.TransformVector(relativeOffset);
        line = LineMgr.inst.CreateFollowLine(entity.position, targetEntity.position + offset, targetEntity.position);
        line.gameObject.SetActive(false);

        startPosition = entity.position;
    }

    public float followThreshold = 25000;
    public Vector3 offset;
    // Update is called once per frame
    public override void Tick()
    {
        offset = targetEntity.transform.TransformVector(relativeOffset);
        movePosition = targetEntity.transform.position + offset;
        //entity.desiredHeading = ComputePredictiveDH(relativeOffset);
        //entity.desiredHeading = ComputeDHDS().dh;
        if (diff.sqrMagnitude < followThreshold) {
            entity.desiredSpeed = targetEntity.speed;
            entity.desiredHeading = targetEntity.heading;
        } else {
            DHDS dhds;
            if (useRegular)
                dhds = ComputeDHDS();
            else if (AIMgr.inst.movementType == MovementType.PotentialFields)
                dhds = ComputePotentialDHDS();
            else if (AIMgr.inst.movementType == MovementType.VelocityObstacles)
                dhds = ComputeVODHDS(entity, EntityMgr.inst.entities);
            else
                dhds = ComputeDHDS();
            entity.desiredSpeed = dhds.ds;
            entity.desiredHeading = dhds.dh;
        }

        commandTime += Time.deltaTime;
        distanceTraveled += entity.speed * Time.deltaTime;

    }

    public bool done = false;//user can set it to done

    public bool caughtUp = false;
    public float timeSinceFollowingThreshold = 0;
    public float distanceSinceFollowingThreshold = 0;
    public Vector3 startPositionSinceFollowing = Vector3.zero;
    public bool fromCaughtUp;
    public override bool IsDone()
    {
        if (!caughtUp && diff.sqrMagnitude < followThreshold)
        {
            timeSinceFollowingThreshold = commandTime + timeThreshold;
            distanceSinceFollowingThreshold = distanceTraveled + distanceThreshold;
            startPositionSinceFollowing = entity.position;
            caughtUp = true;
        }

        if (condition == CommandCondition.InRangeOfASpecificEntity)
        {
            if (fromCaughtUp)
            {
                if (caughtUp)
                    return done || ((entity.position - conditionEntity.position).sqrMagnitude < (distanceThreshold * distanceThreshold));
                else
                    return done;
            }
            else
                return done || ((entity.position - conditionEntity.position).sqrMagnitude < (distanceThreshold * distanceThreshold));
        }
        else if (condition == CommandCondition.InRangeOfTypeOfEntity)
        {
            if (fromCaughtUp)
            {
                if (caughtUp)
                    return done || CheckIfEntityTypeInRange(conditionEntityType, distanceThreshold);
                else
                    return done;
            }
            else
                return done || CheckIfEntityTypeInRange(conditionEntityType, distanceThreshold);
        }
        else if (condition == CommandCondition.TimeFromCommandStart)
        {
            if (fromCaughtUp)
            {
                if (caughtUp)
                    return done || commandTime > timeSinceFollowingThreshold;
                else
                    return done;
            }
            else
                return done || (commandTime > timeThreshold);
        }
        else if (condition == CommandCondition.DistanceTraveled)
        {
            if (fromCaughtUp)
            {
                if (caughtUp)
                    return done || distanceTraveled > distanceSinceFollowingThreshold;
                else
                    return done;
            }
            else
                return done || (distanceTraveled > distanceThreshold);
        }
        else if (condition == CommandCondition.DistanceFromStart)
        {
            if (fromCaughtUp)
            {
                if (caughtUp)
                    return done || ((entity.position - startPositionSinceFollowing).sqrMagnitude > distanceThreshold * distanceThreshold);
                else
                    return done;
            }
            else
                return done || ((entity.position - startPosition).sqrMagnitude > distanceThreshold * distanceThreshold);
        }
        /*else if (condition == CommandCondition.TimeFromFollowing)
        {
            if (!caughtUp && diff.sqrMagnitude < followThreshold)
            {
                caughtUp = true;
                timeSinceFollowingThreshold = commandTime + timeThreshold;
            }
            if (caughtUp)
                return done || commandTime > timeSinceFollowingThreshold;
            else
                return done;
        }
        else if (condition == CommandCondition.DistanceFromFollowing)
        {
            if (!caughtUp && diff.sqrMagnitude < followThreshold)
            {
                caughtUp = true;
                distanceSinceFollowingThreshold = distanceTraveled + distanceThreshold;
            }
            if (caughtUp)
                return done || distanceTraveled > distanceSinceFollowingThreshold;
            else
                return done;
        }*/
        else
            return done;
    }

    public override void Stop()
    {
        base.Stop();
        entity.desiredSpeed = 0;
        LineMgr.inst.DestroyLR(line);
    }

    Vector3 relativeVelocity;
    public float predictedInterceptTime;
    public Vector3 predictedMovePosition;
    Vector3 predictedDiff;
    //------------------------------------------------------
    public float ComputePredictiveDH(Vector3 relativeOffset)
    {
        float dh;
        movePosition = targetEntity.position + targetEntity.transform.TransformVector(relativeOffset);
        diff = movePosition - entity.position; 
        relativeVelocity = entity.velocity - targetEntity.velocity;
        predictedInterceptTime = diff.magnitude / relativeVelocity.magnitude;
        if (predictedInterceptTime >= 0) {
            predictedMovePosition = movePosition + (targetEntity.velocity * predictedInterceptTime);
            predictedDiff = predictedMovePosition - entity.position;
            dh = Utils.Degrees360(Mathf.Atan2(predictedDiff.x, predictedDiff.z) * Mathf.Rad2Deg);
        } else {
            dh = ComputeDHDS().dh;
        }
        return dh;
    }

}
