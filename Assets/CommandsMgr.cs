using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsMgr : MonoBehaviour
{
    [Header("Command Parameters")]
    public CommandCondition commandCondition;
    public float distanceThreshold;
    public float timeThreshold;
    public Entity381 entity;
    public EntityType entityType;

    [Header("Menu")]
    public GameObject commandMenu;

    public static CommandsMgr inst;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;

        commandCondition = CommandCondition.NoCondition;
        distanceThreshold = 1000;
        timeThreshold = 60;
        entity = null;
        entityType = EntityType.DDG51;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            commandMenu.SetActive(!commandMenu.activeSelf);
        }
    }

    public void SetCommandCondition(int input)
    {
        commandCondition = (CommandCondition) input;
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
        entity = null;

        foreach(Entity381 ent in EntityMgr.inst.entities)
        {
            if (ent.name.ToLower().Equals(input.ToLower()))
            {
                entity = ent;
                break;
            }
        }
    }

    public void SetEntityType(int input)
    {
        entityType = (EntityType) input;
    }
}
