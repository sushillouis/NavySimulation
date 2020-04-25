using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        print("Mouse down");
        if (Input.GetMouseButtonDown(1)) {
            print("Right mouse button clicked");
        }
    }

    public GameObject entity;
    private void OnMouseOver()
    {
        // print("Mouse is over: " + entity.name);
    }

}
