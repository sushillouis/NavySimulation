using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specular : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetOrientation();
    }

    public void SetOrientation()
    {
        GameObject light = GameObject.Find("Directional Light");
        transform.localEulerAngles = light.transform.eulerAngles + offset;
    }

    public Vector3 offset;
}
