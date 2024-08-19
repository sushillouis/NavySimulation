using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public EntityData entity;

    public List<int> entityIndex;
    public List<int> entityType;
    public List<Vector3> position;
    public List<Vector3> velocity;
    public List<float> speed;
    public List<float> ds;
    public List<float> heading;
    public List<float> dh;


    public GameData()
    {
        entityIndex = new List<int>();
        entityType = new List<int>();
        position = new List<Vector3>();
        velocity = new List<Vector3>();
        speed = new List<float>();
        ds = new List<float>();
        heading = new List<float>();
        dh = new List<float>();
    }

    public void Clear()
    {
        entityIndex.Clear();
        entityType.Clear();
        position.Clear();
        velocity.Clear();
        speed.Clear();
        ds.Clear();
        heading.Clear();
        dh.Clear();
    }
}
