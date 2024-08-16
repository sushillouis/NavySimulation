using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData
{
    public int entityIndex;
    public int entityType;
    public float[] position;
    public float[] velocity;
    public float speed;
    public float ds;
    public float heading;
    public float dh;

    public EntityData(Entity381 entity)
    {
        entityIndex = EntityMgr.inst.entities.IndexOf(entity);
        entityType = (int) entity.entityType;

        position = new float[3];
        position[0] = entity.position.x;
        position[1] = entity.position.y; 
        position[2] = entity.velocity.z;

        velocity = new float[3];
        velocity[0] = entity.velocity.x;
        velocity[1] = entity.velocity.y;
        velocity[2] = entity.velocity.z;

        speed = entity.speed;
        ds = entity.desiredSpeed;
        heading = entity.heading;
        dh = entity.desiredHeading;
    }


}
