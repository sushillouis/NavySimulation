using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCameraMgr : MonoBehaviour
{
    public GameObject vrCameraRig;
    public GameObject vrCamera;

    public static VRCameraMgr inst;

    public float cameraMoveSpeed = 500;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 moveVector;
        float x = VRControlMgr.inst.leftThumbstickMove.action.ReadValue<Vector2>().x * Time.deltaTime * cameraMoveSpeed;
        float y = VRControlMgr.inst.leftThumbstickMove.action.ReadValue<Vector2>().y * Time.deltaTime * cameraMoveSpeed;

        if (VRControlMgr.inst.leftGripPress.action.IsPressed())
            moveVector = new Vector3(0, y, 0);
        else
        {
            Vector3 forward = y * new Vector3(vrCamera.transform.forward.x, 0, vrCamera.transform.forward.z);
            Vector3 right = x * new Vector3(vrCamera.transform.right.x, 0, vrCamera.transform.right.z);
            moveVector =  forward + right;
        }
            

        vrCameraRig.transform.Translate(moveVector);

        vrCameraRig.transform.position = new Vector3(vrCameraRig.transform.position.x, Mathf.Clamp(vrCameraRig.transform.position.y, 100, 1000), vrCameraRig.transform.position.z);
    }
}
