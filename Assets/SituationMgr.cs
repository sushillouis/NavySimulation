using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SituationMgr : MonoBehaviour
{
    public Entity381 CVN75;
    public Entity381 DDG51;
    public Entity381 accomplice;
    public List<Entity381> droneList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            ResetEntities();
            SpawnScenario();
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
        Vector3 ddgSpawmPos = cvnSpawnPos + CVN75.transform.right * 770; 
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
        Vector3 cornerDronePos = accSpawmPos - accomplice.transform.right * 700;
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

        DistanceMgr.inst.Initialize();
        VOMgr.inst.Initialize();
    }

    void SetInitialWaypoints()
    {
        //Traffic Ships
        TrafficMgr.inst.SetWaypoints();

        //CVN75

        //DDG51

        //Accomplice

        //Drones

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
