using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VO
{
    float theta;
    float delta;
    public float minusDelta;
    public float plusDelta;
    public bool giveWay;

    public Entity ownship;
    public Entity target;

    public float collisionRadius;

    public VO(Entity ownship, Entity target)
    {
        this.ownship = ownship;
        this.target = target;
        giveWay = IsGiveWay(ownship, target);
        collisionRadius = AIMgr.inst.collisionRadius;
    }

    public bool IsGiveWay(Entity ownship, Entity target)
    {
        Vector3 diff = target.position - ownship.position;
        float rhown = (Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg) - ownship.heading;
        float rht = (Mathf.Atan2(-diff.x, -diff.z) * Mathf.Rad2Deg) - target.heading;

        if (!Utils.AngleBetween(rhown, 112.5f, 350f))
            return true;
        else return Utils.Degrees360(rhown - 112.5f) > Utils.Degrees360(rht - 112.5f); //not sure if this is right
    }

    public void CalcVO()
    {
        Vector3 diff = target.position - ownship.position;

        float relPosAngle = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg; //theta
        float maxVOAngle = Mathf.Asin(collisionRadius / diff.magnitude) * Mathf.Rad2Deg; //delta

        theta = relPosAngle;
        delta = maxVOAngle;

        plusDelta = Utils.Degrees360(theta + delta);
        minusDelta = Utils.Degrees360(theta - delta);

        giveWay = IsGiveWay(ownship, target);

    }


    public float CalcAlpha(Vector3 velocity)
    {
        Vector3 relVelOwn = velocity - target.velocity;
        float relVelAngle = Mathf.Atan2(relVelOwn.x, relVelOwn.z) * Mathf.Rad2Deg; //alpha

        return relVelAngle;
    }
}

public class VOMgr : MonoBehaviour
{
    public bool isInitialized = false;
    public Dictionary<Entity, Dictionary<Entity, VO>> vosDictionary;
    public static VOMgr inst;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (AIMgr.inst.movementType == MovementType.VelocityObstacles)
        {
            if (!isInitialized)
                Initialize();
            else
                UpdateVOs();
        }
    }

    public void Initialize()
    {
        isInitialized = true;
        vosDictionary = new Dictionary<Entity, Dictionary<Entity, VO>>();

        foreach (Entity ownship in EntityMgr.inst.entities)
        {
            Dictionary<Entity, VO> ownshipDictionary = new Dictionary<Entity, VO>();
            vosDictionary.Add(ownship, ownshipDictionary);

            foreach (Entity target in EntityMgr.inst.entities)
            {
                VO vo = new VO(ownship, target);
                vo.CalcVO();
                ownshipDictionary.Add(target, vo);
            }
        }
    }

    public void UpdateVOs()
    {
        foreach (Entity ownship in EntityMgr.inst.entities)
        {
            foreach (Entity target in EntityMgr.inst.entities)
            {
                GetVO(ownship, target).CalcVO();
            }
        }
    }

    public VO GetVO(Entity ownship, Entity target)
    {
        VO vo = null;
        if (isInitialized)
            vo = vosDictionary[ownship][target];

        return vo;
    }

}
