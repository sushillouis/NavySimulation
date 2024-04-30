using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecularController : MonoBehaviour
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
        specular.transform.localEulerAngles = transform.eulerAngles + offset;
    }

    public GameObject specular;
    Vector3 offset = new Vector3(11.618f, 2, 0);
}
