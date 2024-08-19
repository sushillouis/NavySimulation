using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EntityType
{
    DDG51,
    Container,
    MineSweeper,
    OilServiceVessel,
    OrientExplorer,
    PilotVessel,
    SmitHouston,
    Tanker,
    TugBoat,
    JARIUSV,
    SeaHunter,
    Mykola,
    SeaBaby,
    CVN75
}


public class Entity381 : MonoBehaviour
{
    //------------------------------
    // values that change while running
    //------------------------------
    public bool isSelected = false;
    public Vector3 position = Vector3.zero;
    public Vector3 velocity = Vector3.zero;

    public float speed;
    public float desiredSpeed;
    public float heading; //degrees
    public float desiredHeading; //degrees
    //------------------------------
    // values that do not change
    //------------------------------
    public float acceleration;
    public float turnRate;
    public float maxSpeed;
    public float minSpeed;
    public float mass;
    public float length;

    public EntityType entityType;

    public GameObject cameraRig;
    public GameObject selectionCircle;

    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
        cameraRig = transform.Find("CameraRig").gameObject;
        selectionCircle = transform.Find("Decorations").Find("SelectionCylinder").gameObject;
    }

    // Update is called once per frame
    public void LoadData(GameData data)
    {
        /*
        entityIndex = data.entityIndex[0];
        entityType = (EntityType) data.entityType[0];
        position = data.position[0];
        velocity = data.velocity[0];
        speed = data.speed[0];
        desiredSpeed = data.ds[0];
        heading = data.heading[0];
        desiredHeading = data.dh[0];
        */
    }

    public void SaveData(GameData data)
    {
        Debug.Log("save test");
        EntityMgr.inst.entities.IndexOf(this);



        data.entityIndex.Add(EntityMgr.inst.entities.IndexOf(this));
        data.entityType.Add((int) entityType);
        data.position.Add(position);
        data.velocity.Add(velocity);
        data.speed.Add(speed);
        data.ds.Add(desiredSpeed);
        data.heading.Add(heading);
        data.dh.Add(desiredHeading);
    }


}
