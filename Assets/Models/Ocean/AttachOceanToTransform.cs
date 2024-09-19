using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachOceanToTransform : MonoBehaviour
{
    public Transform targetTransform;
    private Vector3 _pos;

    private void Awake()
    {
        targetTransform = Camera.main.transform;
    }
    // Update is called once per frame
    void Update()
    {
        _pos.x = targetTransform.transform.position.x;
        _pos.z = targetTransform.transform.position.z;
        transform.position = _pos;
    }
}
