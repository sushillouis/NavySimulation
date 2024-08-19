using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CommandData
{
    public int entityIndex;
    public int commandsIndex;
    public int startCommandsIndex;
    public float[] movePostion;

    public float[] startPosition;
    public float commandTime;
    public float distanceTraveled;
    public float distanceThreshold;
    public float timeThreshold;
    public int conditionEntityIndex;
    public int conditionEntityType;
    public int endCondition;

    public int startCondition;
    public bool startCommand;
    public bool clearQueueWhenStart;
    public bool insertWhenAdded;
    public float startDistanceThreshold;
    public int startConditionEntityIndex;
    public int startConditionEntityType;

    public CommandData(Command command)
    {
        Entity381 ent = command.entity;

        entityIndex = EntityMgr.inst.entities.IndexOf(ent);
        commandsIndex = ent.GetComponent<UnitAI>().commands.IndexOf(command);
        startCommandsIndex = ent.GetComponent<UnitAI>().startCommands.IndexOf(command);

        movePostion = new float[3];
        movePostion[0] = command.movePosition.x;
        movePostion[1] = command.movePosition.y;
        movePostion[2] = command.movePosition.z;

        startPosition = new float[3];
        startPosition[0] = command.startPosition.x;
        startPosition[1] = command.startPosition.y;
        startPosition[2] = command.startPosition.z;

        commandTime = command.commandTime;
        distanceTraveled = command.distanceTraveled;
        distanceThreshold = command.distanceThreshold;
        timeThreshold = command.timeThreshold;
        conditionEntityIndex = EntityMgr.inst.entities.IndexOf(command.conditionEntity);
        conditionEntityType = (int) command.conditionEntityType;
        endCondition = (int) command.condition;

        startCondition = (int) command.startCondition;
        startCommand = command.startCommand;
        clearQueueWhenStart = command.clearQueueWhenStart;
        insertWhenAdded = command.insertWhenAdded;
        startDistanceThreshold = command.startDistanceThreshold;
        startConditionEntityIndex = EntityMgr.inst.entities.IndexOf(command.startConditionEntity);
        startConditionEntityType = (int) command.startConditionEntityType;

    }
}
