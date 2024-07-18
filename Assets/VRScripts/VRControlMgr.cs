using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRControlMgr : MonoBehaviour
{

    public static VRControlMgr inst;

    [Header("Right Controller Inputs")]
    public InputActionProperty rightTriggerPress;
    public InputActionProperty rightGripPress;
    public InputActionProperty AButtonPress;
    public InputActionProperty BButtonPress;
    public InputActionProperty rightThumbstickMove;

    [Header("Left Controller Inputs")]
    public InputActionProperty leftTriggerPress;
    public InputActionProperty leftGripPress;
    public InputActionProperty XButtonPress;
    public InputActionProperty YButtonPress;
    public InputActionProperty leftThumbstickMove;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (VRControlMgr.inst.rightTriggerPress.action.WasPressedThisFrame()){
            Debug.Log("Started");
        }
        if (VRControlMgr.inst.rightTriggerPress.action.WasCompletedThisFrame()){
            Debug.Log("Finished");
        }
    }
}
