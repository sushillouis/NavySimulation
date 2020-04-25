using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move : Command
{
    public Vector3 movePosition;
    public Move(Entity381 ent, Vector3 pos) : base(ent)
    {
        movePosition = pos;
    }

    public override void Init()
    {
        //Debug.Log("MoveInit:\tMoving to: " + movePosition);
        line = LineMgr.inst.CreateMoveLine(entity.position, movePosition);
        line.gameObject.SetActive(false);
    }

    public override void Tick()
    {
        entity.desiredHeading = ComputeDH();
        entity.desiredSpeed = entity.maxSpeed;
        line.SetPosition(1, movePosition);
    }

    public Vector3 diff = Vector3.positiveInfinity;
    public float dhRadians;
    public float dhDegrees;
    public float ComputeDH()
    {
        diff = movePosition - entity.position;
        dhRadians = Mathf.Atan2(diff.x, diff.z);
        dhDegrees = Utils.Degrees360(Mathf.Rad2Deg * dhRadians);
        return dhDegrees;
    }


    public float ComputePotentialDHDS()
    {
        return 0;
    }

    public float doneDistanceSq = 1000;
    public override bool IsDone()
    {
        return (diff.sqrMagnitude < doneDistanceSq);
    }

    public override void Stop()
    {
        entity.desiredSpeed = 0;
        LineMgr.inst.DestroyLR(line);
    }
}
