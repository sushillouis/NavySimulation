using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour, IDataPersistence
{
    public int id;
    public int entityIndex;
    public int entityType;
    public Vector3 position;
    public Vector3 velocity;
    public float speed;
    public float ds;
    public float heading;
    public float dh;

    // Start is called before the first frame update
    void Start()
    {
        id = Random.Range(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(GameData data)
    {
        EntityMgr.inst.ResetEntities();

        EntityMgr.entityId = data.entityID - (data.entityIndex.Count);

        for(int i = 0; i < data.entityIndex.Count; i++)
        {
            Entity381 ent = EntityMgr.inst.CreateEntity((EntityType) data.entityType[i], data.position[i], Vector3.zero);
            ent.velocity = data.velocity[i];
            ent.speed = data.speed[i];
            ent.desiredSpeed = data.ds[i];
            ent.heading = data.heading[i];
            ent.desiredHeading = data.dh[i];
        }
}

    public void SaveData(GameData data)
    {
        Entity381 entity = EntityMgr.inst.entities[id];

        data.Clear();

        Debug.Log("save test");

        data.entityID = EntityMgr.entityId;

        foreach(Entity381 ent in EntityMgr.inst.entities)
        {
            data.entityIndex.Add(EntityMgr.inst.entities.IndexOf(ent));
            data.entityType.Add((int)ent.entityType);
            data.position.Add(ent.position);
            data.velocity.Add(ent.velocity);
            data.speed.Add(ent.speed);
            data.ds.Add(ent.desiredSpeed);
            data.heading.Add(ent.heading);
            data.dh.Add(ent.desiredHeading);
        };
    }
}
