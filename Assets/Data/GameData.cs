using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int entityID;

    //Entity Data Lists
    public List<int> entityIndex;
    public List<int> entityType;
    public List<Vector3> position;
    public List<Vector3> velocity;
    public List<float> speed;
    public List<float> ds;
    public List<float> heading;
    public List<float> dh;

    //Command Data Lists
    public List<int> commandType;
    public List<int> commandEntIndex;
    public List<int> commandIndex;
    public List<bool> isRunning;
    public List<Vector3> movePosition;

    public List<int> followTargetIndex;
    public List<Vector3> followOffset;

    public List<Vector3> startPosition;
    public List<float> commandTime;
    public List<float> distanceTraveled;
    public List<float> distanceThreshold;
    public List<float> timeThreshold;
    public List<int> conditionEntIndex;
    public List<int> conditionEntityType;
    public List<int> endCondition;

    public List<int> startCondition;
    public List<bool> startCommand;
    public List<bool> clearQueueWhenStart;
    public List<bool> insertWhenAdded;
    public List<float> startDistanceThreshold;
    public List<int> startConditionEntIndex;
    public List<int> startConditionEntityType;

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

        commandType = new List<int>();
        commandEntIndex = new List<int>();
        commandIndex = new List<int>();
        isRunning = new List<bool>();
        movePosition = new List<Vector3>();

        followTargetIndex = new List<int>();
        followOffset = new List<Vector3>();

        startPosition = new List<Vector3>();
        commandTime = new List<float>();
        distanceTraveled = new List<float>();
        distanceThreshold = new List<float>();
        timeThreshold = new List<float>();
        conditionEntIndex = new List<int>();
        conditionEntityType = new List<int>();
        endCondition = new List<int>();

        startCondition = new List<int>();
        startCommand = new List<bool>();
        clearQueueWhenStart = new List<bool>();
        insertWhenAdded = new List<bool>();
        startDistanceThreshold = new List<float>();
        startConditionEntIndex = new List<int>();
        startConditionEntityType = new List<int>();
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

        commandType.Clear();
        commandEntIndex.Clear();
        commandIndex.Clear();
        isRunning.Clear();
        movePosition.Clear();

        followOffset.Clear();
        followTargetIndex.Clear();

        startPosition.Clear();
        commandTime.Clear();
        distanceTraveled.Clear();
        distanceThreshold.Clear();
        timeThreshold.Clear();
        conditionEntIndex.Clear();
        conditionEntityType.Clear();
        endCondition.Clear();

        startCondition.Clear();
        startCommand.Clear();
        clearQueueWhenStart.Clear();
        insertWhenAdded.Clear();
        startDistanceThreshold.Clear();
        startConditionEntIndex.Clear();
        startDistanceThreshold.Clear();
    }
}
