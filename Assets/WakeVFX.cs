using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WakeVFX : MonoBehaviour
{
    public VisualEffect wake;
    public Entity381 entity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(entity != null)
        {
            transform.position = entity.transform.position;
            wake.SetVector3("Position", entity.position);
            wake.SetVector3("MiddleVelocity", -entity.velocity);
        }
        
    }
}
