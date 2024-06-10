using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class TrafficMgr : MonoBehaviour
{
    public int numShips;
    public Vector3 laneCenter;
    public Vector2 laneDimensions;
    public float laneOrientation;
    public GameObject test;
    public GameObject cube;

    public List<Entity381> TrafficShips;

    public static TrafficMgr inst;

    void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    { 
        Test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    void SetSpawnCorners()
    {
        float x = laneDimensions.x / 2;
        float y = laneDimensions.y / 2;
        float theta = Utils.Degrees360(Mathf.Atan2(x, y) * Mathf.Rad2Deg);
        float c = new Vector2(x, y).magnitude;
        float topRightAngle = Utils.Degrees360(laneOrientation + theta);
        float topLeftAngle = Utils.Degrees360(laneOrientation - theta);
        float bottomRightAngle = Utils.Degrees360(laneOrientation + 180 - theta);
        float bottomLeftAngle = Utils.Degrees360(laneOrientation + 180 - theta);

        Vector3 topRightDirec = new Vector3(Mathf.Sin(topRightAngle * Mathf.Deg2Rad), 0, Mathf.Cos(topRightAngle * Mathf.Deg2Rad)).normalized;
        Vector3 topLeftDirec = new Vector3(Mathf.Sin(topLeftAngle * Mathf.Deg2Rad), 0, Mathf.Cos(topLeftAngle * Mathf.Deg2Rad)).normalized;
        Vector3 bottomRightDirec = new Vector3(Mathf.Sin(bottomRightAngle * Mathf.Deg2Rad), 0, Mathf.Cos(bottomRightAngle * Mathf.Deg2Rad)).normalized;
        Vector3 bottomLeftDirec = new Vector3(Mathf.Sin(bottomLeftAngle * Mathf.Deg2Rad), 0, Mathf.Cos(bottomLeftAngle * Mathf.Deg2Rad)).normalized;

        topRightCorner = laneCenter + (topRightDirec * c);
        topLeftCorner = laneCenter + (topLeftDirec * c);
        bottomRightCorner = laneCenter + (bottomRightDirec * c);
        bottomLeftCorner = laneCenter + (bottomLeftDirec * c);
    }
    */

    void Test()
    {
        laneOrientation = Utils.Degrees360(laneOrientation);
        test.transform.position = laneCenter;
        test.transform.eulerAngles = new Vector3 (0, laneOrientation, 0);
        BoxCollider bounds = test.GetComponent<BoxCollider>();
        bounds.size = new Vector3(laneDimensions.x, 0,  laneDimensions.y);
        for(int i = 0; i < numShips; i++)
        {
            EntityType vesselType = (EntityType)Random.Range(0, 9);
            GameObject tmp = Instantiate(cube, test.transform);
            tmp.transform.localPosition = RandomPointInBounds(bounds.bounds);
            Entity381 entity = EntityMgr.inst.CreateEntity(vesselType, Vector3.zero, Vector3.zero);
            entity.transform.position = tmp.transform.position;
            Destroy(tmp);

            if(i % 2 == 0)
            {
                entity.heading = laneOrientation;
                entity.desiredHeading = laneOrientation;
            }
            else
            {
                entity.heading = Utils.Degrees360(laneOrientation - 180);
                entity.desiredHeading = Utils.Degrees360(laneOrientation - 180);
            }

            TrafficShips.Add(entity);
        }
    }

    void SetWaypoints()
    {
        for (int i = 0; i < TrafficShips.Count; i++)
        {
            Entity381 entity = TrafficShips[i];
            float angle;
            if (i % 2 == 0)
                angle = laneOrientation;
            else
                angle = Utils.Degrees360(laneOrientation - 180);

            Vector3 direc = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)).normalized;
            Vector3 waypoint = entity.position + direc * 100000;
            List<Entity381> ent0List = new List<Entity381>();
            ent0List.Add(entity);
            AIMgr.inst.HandleMove(ent0List, waypoint);
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
