﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class CameraMgr : MonoBehaviour
{
    public static CameraMgr inst;
    private GameInputs input;
    private Vector3 moveVector;
    private float yawValue;
    private float pitchValue;
    private float prevPinchMag = 0;

    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        input = new GameInputs();
        input.Enable();

        //handles touchscreen zoom
        var touch0pos = new InputAction(type: InputActionType.Value, binding: "<Touchscreen>/touch0/position");
        touch0pos.Enable();
        var touch1pos = new InputAction(type: InputActionType.Value, binding: "<Touchscreen>/touch1/position");
        touch1pos.Enable();
        touch1pos.performed += _ =>
        {
            if (numTouches == 2)
            {
                float mag = (touch0pos.ReadValue<Vector2>() - touch1pos.ReadValue<Vector2>()).magnitude;
                if (prevPinchMag == 0)
                    prevPinchMag = mag;
                float diff = mag - prevPinchMag;
                prevPinchMag = mag;
                CamZoom(-diff * 0.1f);
            }
            else
            {
                prevPinchMag = 0;
            }
        };
    }

    public GameObject RTSCameraRig;
    public GameObject YawNode;   // Child of RTSCameraRig
    public GameObject PitchNode; // Child of YawNode
    public GameObject RollNode;  // Child of PitchNode
    public Camera camera;
    //Camera is child of RollNode

    public float cameraMoveSpeed = 500;
    public float cameraTurnRate = 10;
    public Vector3 currentYawEulerAngles = Vector3.zero;
    public Vector3 currentPitchEulerAngles = Vector3.zero;
    public int numTouches;
    // Update is called once per frame
    void Update()
    {
        numTouches = Touch.activeTouches.Count;
        moveVector = Vector3.zero;
        if(numTouches == 0 || numTouches == 3)
        {
            moveVector += input.Camera.Movement.ReadValue<Vector3>();
        }
        if (numTouches == 0 || numTouches == 4)
        {
            moveVector += input.Camera.TouchUpAndDown.ReadValue<Vector3>();
        }
        if (numTouches == 0 || (numTouches == 1 && !UIMgr.inst.isActive))
        {
            yawValue = input.Camera.Yaw.ReadValue<float>();
            pitchValue = input.Camera.Pitch.ReadValue<float>();
        }


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

    private void CamZoom(float increment)
    {
        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView + increment, 0, 100);
    }

}
