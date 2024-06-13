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
        //SpawnShips();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //SetWaypoints();
        }
    }

    public void SpawnShips()
    {
        laneOrientation = Utils.Degrees360(laneOrientation);
        test.transform.position = laneCenter;
        test.transform.eulerAngles = new Vector3 (0, laneOrientation, 0);

        BoxCollider bounds = test.GetComponent<BoxCollider>();
        bounds.size = new Vector3(laneDimensions.x, 0,  laneDimensions.y);

        List<Vector3> postions = new List<Vector3>();

        for(int i = 0; i < numShips; i++)
        {
            bool spreadOut = false;
            Vector3 localPosition = Vector3.zero;

            while (!spreadOut)
            {
                localPosition = RandomPointInBounds(bounds.bounds);
                spreadOut = true;
                foreach(Vector3 shipPos in postions)
                {
                    if(Vector3.Distance(shipPos, localPosition) < 2 * AIMgr.inst.collisionRadius)
                    {
                        spreadOut = false;
                        break;
                    }
                }
            }

            postions.Add(localPosition);

            GameObject tmp = Instantiate(cube, test.transform);
            tmp.transform.localPosition = localPosition;

            EntityType vesselType = (EntityType)Random.Range(0, 9);
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

    public void SetWaypoints()
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
