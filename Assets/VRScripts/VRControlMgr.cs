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
        bool trigger = leftTriggerPress.action.IsPressed();
        bool grip = leftGripPress.action.IsPressed();
        bool xButton = XButtonPress.action.IsPressed();
        bool yButton = YButtonPress.action.IsPressed();
        Vector2 thumbstick = leftThumbstickMove.action.ReadValue<Vector2>();

        if (trigger)
            Debug.Log("Left Trigger");
        if (grip)
            Debug.Log("Left Grip");
        if (xButton)
            Debug.Log("X Button");
        if (yButton)
            Debug.Log("Y Button");
        if (thumbstick != Vector2.zero)
            Debug.Log("Left Thumbstick");
    }
}
