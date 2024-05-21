using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TestMgr : MonoBehaviour
{

    public static TestMgr inst;
    bool added = true;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!added)
        {
            HeadOnWaypoints();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetEntities();
            HeadOn();
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
        camera.transform.position = new Vector3(5500, 300, 1200);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, 270, 0);

        Entity381 ent0 = EntityMgr.inst.CreateEntity(EntityType.DDG51, new Vector3(4500, 0, 1200), Vector3.zero);
        ent0.desiredHeading = 270f;
        ent0.heading = 270f;

        Entity381 ent1 = EntityMgr.inst.CreateEntity(EntityType.DDG51, new Vector3(2500, 0, 3200), Vector3.zero);
        ent1.desiredHeading = 180f;
        ent1.heading = 180f;

        VOMgr.inst.ownship = ent0;
        VOMgr.inst.target = ent1;
        VOMgr.inst.test = new VO(ent0, ent1);

        added = false;

        DistanceMgr.inst.Initialize();
    }

    public void Crossing90Waypoints() 
    {
        List<Entity381> ent0List = new List<Entity381>();
        ent0List.Add(VOMgr.inst.ownship);
        AIMgr.inst.HandleMove(ent0List, new Vector3(500, 0, 1200));
        added = true;

        List<Entity381> ent1List = new List<Entity381>();
        ent1List.Add(VOMgr.inst.target);
        AIMgr.inst.HandleMove(ent1List, new Vector3(2500, 0, 200));
        added = true;
    }

    public void Overtaking()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.position = new Vector3(5500, 300, 1200);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, 270, 0);

        Entity381 ent0 = EntityMgr.inst.CreateEntity(EntityType.DDG51, new Vector3(4500, 0, 1200), Vector3.zero);
        ent0.desiredHeading = 270f;
        ent0.heading = 270f;

        Entity381 ent1 = EntityMgr.inst.CreateEntity(EntityType.DDG51, new Vector3(3500, 0, 1200), Vector3.zero);
        ent1.desiredHeading = 270f;
        ent1.heading = 270f;
        ent1.maxSpeed = 25;

        VOMgr.inst.ownship = ent0;
        VOMgr.inst.target = ent1;
        VOMgr.inst.test = new VO(ent0, ent1);

        added = false;

        DistanceMgr.inst.Initialize();
    }

    public void OvertakingWaypoints()
    {
        List<Entity381> ent0List = new List<Entity381>();
        ent0List.Add(VOMgr.inst.ownship);
        AIMgr.inst.HandleMove(ent0List, new Vector3(-1500, 0, 1200));
        added = true;

        List<Entity381> ent1List = new List<Entity381>();
        ent1List.Add(VOMgr.inst.target);
        AIMgr.inst.HandleMove(ent1List, new Vector3(-1500, 0, 1200));
        added = true;
    }

    public void HeadOn()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.position = new Vector3(5500, 300, 1200);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, 270, 0);

        Entity381 ent0 = EntityMgr.inst.CreateEntity(EntityType.DDG51, new Vector3(4500, 0, 1200), Vector3.zero);
        ent0.desiredHeading = 270f;
        ent0.heading = 270f;

        Entity381 ent1 = EntityMgr.inst.CreateEntity(EntityType.DDG51, new Vector3(3500, 0, 1200), Vector3.zero);
        ent1.desiredHeading = 90f;
        ent1.heading = 90f;

        VOMgr.inst.ownship = ent0;
        VOMgr.inst.target = ent1;
        VOMgr.inst.test = new VO(ent0, ent1);

        added = false;

        DistanceMgr.inst.Initialize();
    }

    public void HeadOnWaypoints()
    {
        List<Entity381> ent0List = new List<Entity381>();
        ent0List.Add(VOMgr.inst.ownship);
        AIMgr.inst.HandleMove(ent0List, new Vector3(500, 0, 1200));
        added = true;

        List<Entity381> ent1List = new List<Entity381>();
        ent1List.Add(VOMgr.inst.target);
        AIMgr.inst.HandleMove(ent1List, new Vector3(6500, 0, 1200));
        added = true;
    }
}
