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
    public AudioSource audioSource;
    bool toggleAudio = true;

    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
        cameraRig = transform.Find("CameraRig").gameObject;
        selectionCircle = transform.Find("Decorations").Find("SelectionCylinder").gameObject;
    }

    // Update is called once per frame
    public void Update()
    {
        if(audioSource != null)
        {
            if (speed > 0 && toggleAudio)
            {
                audioSource.Play();
                toggleAudio = false;
            }
            else if(speed == 0)
            {
                audioSource.Stop();
                toggleAudio = true;
            }
        }
    }
}
