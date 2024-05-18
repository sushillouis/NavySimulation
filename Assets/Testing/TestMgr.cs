using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TestMgr : MonoBehaviour
{

    public static TestMgr inst;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetEntities();
            Crossing90();
        }
    }

    public void ResetEntities()
    {
        int count = EntityMgr.inst.entities.Count;
        for (int i = count-1; i >= 0; i--)
        {
            Entity381 ent = EntityMgr.inst.entities[i];
            EntityMgr.inst.entities.RemoveAt(i);
            Destroy(ent.gameObject);
        }
        DistanceMgr.inst.Initialize();
    }

    public void Crossing90()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.localPosition = new Vector3(3500, 0, 2000);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, 270, 0);

        Entity381 ent0 = EntityMgr.inst.CreateEntity(EntityType.DDG51, new Vector3(3500, 0, 1200), Vector3.zero);
        ent0.desiredHeading = 270f;
        ent0.heading = 270f;

        Entity381 ent1 = EntityMgr.inst.CreateEntity(EntityType.DDG51, new Vector3(2500, 0, 2200), Vector3.zero);
        ent1.desiredHeading = 180f;
        ent1.heading = 180f;

        VOMgr.inst.ownship = ent0;
        VOMgr.inst.target = ent1;
        VOMgr.inst.test = new VO(ent0, ent1);

        DistanceMgr.inst.Initialize();
    }
}
