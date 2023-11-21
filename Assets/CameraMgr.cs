using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMgr : MonoBehaviour
{
    public static CameraMgr inst;
    private GameInputs input;
    private Vector3 moveVector;
    private float yawValue;
    private float pitchValue;

    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        input = new GameInputs();
        input.Enable();
    }

    public GameObject RTSCameraRig;
    public GameObject YawNode;   // Child of RTSCameraRig
    public GameObject PitchNode; // Child of YawNode
    public GameObject RollNode;  // Child of PitchNode
    //Camera is child of RollNode

    public float cameraMoveSpeed = 500;
    public float cameraTurnRate = 10;
    public Vector3 currentYawEulerAngles = Vector3.zero;
    public Vector3 currentPitchEulerAngles = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        moveVector = input.Camera.Movement.ReadValue<Vector3>();
        yawValue = input.Camera.Yaw.ReadValue<float>();
        pitchValue = input.Camera.Pitch.ReadValue<float>();


        YawNode.transform.Translate(moveVector * Time.deltaTime * cameraMoveSpeed);
        
        currentYawEulerAngles = YawNode.transform.localEulerAngles;
        currentYawEulerAngles.y +=  yawValue * cameraTurnRate * Time.deltaTime;
        YawNode.transform.localEulerAngles = currentYawEulerAngles;

        currentPitchEulerAngles = PitchNode.transform.localEulerAngles;
        currentPitchEulerAngles.x += pitchValue * cameraTurnRate * Time.deltaTime;
        PitchNode.transform.localEulerAngles = currentPitchEulerAngles;

        if (input.Camera.RTSView.triggered) {
            if (isRTSMode) {
                YawNode.transform.SetParent(SelectionMgr.inst.selectedEntity.cameraRig.transform);
                YawNode.transform.localPosition = Vector3.zero;
                YawNode.transform.localEulerAngles = Vector3.zero;
            } else {
                YawNode.transform.SetParent(RTSCameraRig.transform);
                YawNode.transform.localPosition = Vector3.zero;
                YawNode.transform.localEulerAngles = Vector3.zero;
            }
            isRTSMode = !isRTSMode;
        }
    }
    public bool isRTSMode = true;
}
