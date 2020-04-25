using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Intercept : Follow
{
    public Intercept(Entity381 ent, Entity381 target): base(ent, target, Vector3.zero)
    {
        //Follow does all the work
    }

    public override void Init()
    {
        //Debug.Log("Intercept:\t ing: " + targetEntity.gameObject.name);
        line = LineMgr.inst.CreateInterceptLine(entity.position, targetEntity.position, targetEntity.position);
        line.gameObject.SetActive(false);
    }

    public override void Tick()
    {
        //movePosition = targetEntity.transform.position;
        float dh = ComputePredictiveDH(Vector3.zero);
        entity.desiredHeading = dh;
        entity.desiredSpeed = entity.maxSpeed;
    }

    public override bool IsDone()
    {
        return diff.sqrMagnitude < doneDistanceSq;
    }

    public override void Stop()
    {
        base.Stop();
        entity.desiredSpeed = 0;
        targetEntity.desiredSpeed = 0;
        targetEntity.GetComponentInParent<UnitAI>().StopAndRemoveAllCommands();
        Vector3 deadRot = targetEntity.transform.localEulerAngles;
        deadRot.x = 90;
        targetEntity.transform.localEulerAngles = deadRot;
        LineMgr.inst.DestroyLR(line);
    }

}
