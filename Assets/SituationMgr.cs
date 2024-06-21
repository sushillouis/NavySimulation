using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class SituationMgr : MonoBehaviour
{
    public Entity381 CVN75;
    public Entity381 DDG51;
    public Entity381 accomplice;
    public List<Entity381> droneList;

    public int stage;

    // Start is called before the first frame update
    void Start()
    {
        stage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            if (stage == 0)
            {
                ResetEntities();
                SpawnScenario();
                stage++;
            }
            else if (stage == 1)
            {
                SetInitialWaypoints();
                stage++;
            }
            else if (stage == 2) 
            {
                SetSecondDroneWaypoints();
                stage++;
            }
            else if (stage == 3)
                SetSecondDDG51Waypoint();
        }
    }

    void SpawnScenario()
    {
        //Traffic Ships
        TrafficMgr.inst.SpawnShips();

        //CVN75
        float cvnSpawnAngle = Utils.Degrees360(TrafficMgr.inst.laneOrientation + 90);
        Vector3 direc = new Vector3(Mathf.Sin(cvnSpawnAngle * Mathf.Deg2Rad), 0, Mathf.Cos(cvnSpawnAngle * Mathf.Deg2Rad)).normalized;
        Vector3 cvnSpawnPos = TrafficMgr.inst.laneCenter + direc * 10000;
        CVN75 = EntityMgr.inst.CreateEntity(EntityType.CVN75, cvnSpawnPos, new Vector3(0, cvnSpawnAngle, 0));
        CVN75.heading = cvnSpawnAngle;
        CVN75.desiredHeading = cvnSpawnAngle;

        //DDG51
        Vector3 ddgSpawmPos = cvnSpawnPos + CVN75.transform.right * 770 - CVN75.transform.forward * 770; 
        DDG51 = EntityMgr.inst.CreateEntity(EntityType.DDG51, ddgSpawmPos, new Vector3(0, cvnSpawnAngle, 0));
        DDG51.heading = cvnSpawnAngle;
        DDG51.desiredHeading = cvnSpawnAngle;

        //Accomplice
        float accSpawnAngle = Utils.Degrees360(cvnSpawnAngle + 180);
        Vector3 accSpawmPos = cvnSpawnPos - CVN75.transform.right * 4000;
        accomplice = EntityMgr.inst.CreateEntity(EntityType.Tanker, accSpawmPos, new Vector3(0, accSpawnAngle + 180, 0));
        accomplice.heading = Utils.Degrees360(accSpawnAngle);
        accomplice.desiredHeading = Utils.Degrees360(accSpawnAngle);

        //Drones
        Vector3 cornerDronePos = accSpawmPos - accomplice.transform.right * 1000;
        droneList = new List<Entity381>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector3 droneSpawnPos = cornerDronePos + (100 * i * accomplice.transform.forward) + (100 * j * accomplice.transform.right);
                Entity381 drone = EntityMgr.inst.CreateEntity(EntityType.SeaBaby, droneSpawnPos, Vector3.zero);
                drone.heading = Utils.Degrees360(accSpawnAngle);
                drone.desiredHeading = Utils.Degrees360(accSpawnAngle);
                droneList.Add(drone);
            }
        }

        //Camera
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.position = accSpawmPos + accomplice.transform.forward * 1000 + accomplice.transform.up * 500;
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, accSpawnAngle, 0);

        DistanceMgr.inst.Initialize();
        VOMgr.inst.Initialize();
    }

    void SetInitialWaypoints()
    {
        //Traffic Ships
        TrafficMgr.inst.SetWaypoints();

        //CVN75
        CVN75.desiredSpeed = CVN75.maxSpeed;

        //DDG51
        Follow f0 = new Follow(DDG51, CVN75, new Vector3(770, 0, -770));
        DDG51.GetComponent<UnitAI>().SetCommand(f0);

        //Accomplice
        Vector3 direc = new Vector3(Mathf.Sin(accomplice.heading * Mathf.Deg2Rad), 0, Mathf.Cos(accomplice.heading * Mathf.Deg2Rad)).normalized;
        Vector3 accomplice1stWaypoint = accomplice.position + direc * 10000;
        Vector3 accomplice2ndWaypoint = accomplice1stWaypoint
            + new Vector3(Mathf.Sin(TrafficMgr.inst.laneOrientation * Mathf.Deg2Rad), 0, Mathf.Cos(TrafficMgr.inst.laneOrientation * Mathf.Deg2Rad)).normalized * 100000;
        Move m1 = new Move(accomplice, accomplice1stWaypoint);
        Move m2 = new Move(accomplice, accomplice2ndWaypoint);
        accomplice.GetComponent<UnitAI>().SetCommand(m1);
        accomplice.GetComponent<UnitAI>().AddCommand(m2);

        //Drones
        foreach(Entity381 drone in droneList)
        {
            Follow f1 = new Follow(drone, accomplice, new Vector3(1000, 0, 0));
            drone.GetComponent<UnitAI>().SetCommand(f1);
        }
    }

    void SetSecondDroneWaypoints()
    {
        foreach(Entity381 drone in droneList)
        {
            Follow f1 = new Follow(drone, CVN75, new Vector3(-300, 0, 0));
            drone.GetComponent<UnitAI>().SetCommand(f1);
        }
    }

    void SetSecondDDG51Waypoint()
    {
        Follow f1 = new Follow(DDG51, CVN75, new Vector3(-550, 0, 0));
        f1.useRegular = true;
        DDG51.GetComponent<UnitAI>().SetCommand(f1);
    }

    public void ResetEntities()
    {
        int count = EntityMgr.inst.entities.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            Entity381 ent = EntityMgr.inst.entities[i];
            EntityMgr.inst.entities.RemoveAt(i);
            Destroy(ent.gameObject);
        }
        DistanceMgr.inst.Initialize();
        VOMgr.inst.Initialize();
    }
}
