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
        entityIndex = data.entityIndex[0];
        entityType = data.entityType[0];
        position = data.position[0];
        velocity = data.velocity[0];
        speed = data.speed[0];
        ds = data.ds[0];
        heading = data.heading[0];
        dh = data.dh[0];
}

    public void SaveData(GameData data)
    {
        Entity381 entity = EntityMgr.inst.entities[id];

        Debug.Log("save test");

        /*
        data.entityIndex[0] = (int) entity.entityType;
        data.position[0] = entity.position;
        data.velocity[0] = entity.velocity;
        data.speed[0] = entity.speed;
        data.ds[0] = entity.desiredSpeed;
        data.heading[0] = entity.heading;
        data.dh[0] = entity.desiredHeading;
        */

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
