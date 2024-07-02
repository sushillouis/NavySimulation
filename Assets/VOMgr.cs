using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;
using static UnityStandardAssets.Utility.TimedObjectActivator;

[System.Serializable]
public class VO
{
    float theta;
    float delta;
    public float minusDelta;
    public float plusDelta;
    public bool giveWay;

    public Entity381 ownship;
    public Entity381 target;

    public float collisionRadius;

    public LineRenderer plusDeltaLine;
    public LineRenderer minusDeltaLine;

    public bool visualizationInitialized;
    public bool visualizationEnabled;

    public VO(Entity381 ownship, Entity381 target)
    {
        this.ownship = ownship;
        this.target = target;
        giveWay = IsGiveWay(ownship, target);
        visualizationEnabled = false;
        if(!AIMgr.inst.useSetCollisionRadius)
            collisionRadius = ownship.length + target.length;
        else
            collisionRadius = AIMgr.inst.collisionRadius;
    }

    public bool IsGiveWay(Entity381 ownship, Entity381 target)
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

    public override string ToString()
    {
        string output = "ownship: " + ownship.name;
        output += "\ntarget: " + target.name;
        output += "\nminus delta: " + minusDelta;

        return output;
    }

    public void ToggleVisualization()
    {
        visualizationEnabled = !visualizationEnabled;
    }

    public void UpdateVisualization()
    {
        if (visualizationEnabled)
        {
            if (visualizationInitialized)
                DrawVO();
            else
                InitializeVODrawing();
        }
        else
        {
            if (visualizationInitialized)
            {
                plusDeltaLine.gameObject.SetActive(false);
                minusDeltaLine.gameObject.SetActive(false);
            }
                
        }
    }

    public void InitializeVODrawing()
    {
        //CalcVO();

        plusDeltaLine = LineMgr.inst.CreateVOLine(ownship.position, ownship.position);
        plusDeltaLine.gameObject.SetActive(true);

        minusDeltaLine = LineMgr.inst.CreateVOLine(ownship.position, ownship.position);
        minusDeltaLine.gameObject.SetActive(true);

        visualizationInitialized = true;
    }

    public void DrawVO()
    {

        float plusDeltaAngle = Utils.Degrees360(theta + delta);
        float minusDeltaAngle = Utils.Degrees360(theta - delta);

        Vector3 plusDeltaDirec = new Vector3(Mathf.Sin(plusDeltaAngle * Mathf.Deg2Rad), 0, Mathf.Cos(plusDeltaAngle * Mathf.Deg2Rad)).normalized;
        Vector3 minusDeltaDirec = new Vector3(Mathf.Sin(minusDeltaAngle * Mathf.Deg2Rad), 0, Mathf.Cos(minusDeltaAngle * Mathf.Deg2Rad)).normalized;

        Vector3 plusDeltaEndpoint = ownship.position + (plusDeltaDirec * (ownship.position - target.position).magnitude);
        Vector3 minusDeltaEndpoint = ownship.position + (minusDeltaDirec * (ownship.position - target.position).magnitude);

        plusDeltaLine.SetPosition(0, ownship.position);
        plusDeltaLine.SetPosition(1, plusDeltaEndpoint);
        plusDeltaLine.gameObject.SetActive(true);

        minusDeltaLine.SetPosition(0, ownship.position);
        minusDeltaLine.SetPosition(1, minusDeltaEndpoint);
        minusDeltaLine.gameObject.SetActive(true);

    }
}

public class VOMgr : MonoBehaviour
{
    public Entity381 ownship;
    public Entity381 target;

    public bool isInitialized = false;

    public Dictionary<Entity381, Dictionary<Entity381, VO>> vosDictionary;

    public VO test;

    public static VOMgr inst;
    
    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(AIMgr.inst.movementType == MovementType.VelocityObstacles) 
        {
            if (!isInitialized)
                Initialize();
            else
            {
                UpdateVOs();
                ownship = SelectionMgr.inst.selectedEntity;
                if(ownship != null)
                {
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        foreach (KeyValuePair<Entity381, VO> entry in vosDictionary[ownship])
                        {
                            entry.Value.ToggleVisualization();
                        }
                    }
                    foreach (KeyValuePair<Entity381, VO> entry in vosDictionary[ownship])
                    {
                        entry.Value.UpdateVisualization();
                    }
                }
            }
                
        }
    }

    public void Initialize()
    {
        isInitialized = true;
        vosDictionary = new Dictionary<Entity381, Dictionary<Entity381, VO>>();

        foreach(Entity381 ownship in EntityMgr.inst.entities)
        {
            Dictionary<Entity381, VO> ownshipDictionary = new Dictionary<Entity381, VO>();
            vosDictionary.Add(ownship, ownshipDictionary);

            foreach(Entity381 target in EntityMgr.inst.entities)
            {
                VO vo = new VO(ownship, target);
                vo.CalcVO();
                ownshipDictionary.Add(target, vo);
            }
        }
    }

    public void UpdateVOs()
    {
        foreach(Entity381 ownship in EntityMgr.inst.entities)
        {
            foreach(Entity381 target in EntityMgr.inst.entities)
            {
                GetVO(ownship, target).CalcVO();
            }
        }
    }

    public VO GetVO(Entity381 ownship, Entity381 target)
    {
        VO vo = null;
        if (isInitialized)
            vo = vosDictionary[ownship][target];

        return vo;
    }
}
