using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsMgr : MonoBehaviour
{
    public float distanceThreshold;
    public float timeThreshold;
    public string entityName;
    
    // Start is called before the first frame update
    void Start()
    {
        distanceThreshold = 1000;
        timeThreshold = 60;
        entityName = "none";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDistanceThreshold(string input)
    {
        if(input != "")
        {
            float distance = float.Parse(input);
            distanceThreshold = distance;
        }
        else
            distanceThreshold = 1000;
    }

    public void SetTimeThreshold(string input)
    {
        if (input != "")
        {
            float distance = float.Parse(input);
            timeThreshold = distance;
        }
        else
            timeThreshold = 60;
    }

    public void SetEntityName(string input)
    {
        entityName = "none";

        foreach(Entity381 entity in EntityMgr.inst.entities)
        {
            if (entity.name.ToLower().Equals(input.ToLower()))
            {
                entityName = entity.name;
                break;
            }
        }
    }
}
