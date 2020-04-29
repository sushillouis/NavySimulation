using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DHDS {
    public float dh = 0;//degrees
    public float ds = 0;//m/s
    public DHDS(float h, float s)
    {
        dh = h;
        ds = s;
    }
}




[System.Serializable]
public class Command 
{
    public Entity381 entity;
    public LineRenderer line;
    public bool isRunning = false;
    public Command(Entity381 ent)
    {
        entity = ent;
    }


    // Start is called before the first frame update
    public virtual void Init()
    {
        
    }

    // Update is called once per frame
    public virtual void Tick()
    {

    }

    public virtual bool IsDone()
    {
        return false;
    }

    public virtual void Stop()
    {

    }

}
