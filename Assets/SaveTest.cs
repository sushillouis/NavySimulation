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

        foreach(Entity381 ent in EntityMgr.inst.entities)
        {
            Debug.Log("searchign for " + ent.name + " commands");
            
            List<int> dataIndexes = new List<int>();

            for (int j = 0; j < data.commandEntIndex.Count; j++)
            {
                if (data.commandEntIndex[j] == EntityMgr.inst.entities.IndexOf(ent))
                {
                    Debug.Log("command found");
                    dataIndexes.Add(j);
                }
            }

            foreach (int dataIndex in dataIndexes)
            {
                Debug.Log("command #" + dataIndex);

                Command newCmd;

                if (data.commandType[dataIndex] == 0)
                    newCmd = new Move(EntityMgr.inst.entities[data.commandEntIndex[dataIndex]], data.movePosition[dataIndex]);
                else if (data.commandType[dataIndex] == 1)
                    newCmd = new Follow(EntityMgr.inst.entities[data.commandEntIndex[dataIndex]], EntityMgr.inst.entities[data.followTargetIndex[dataIndex]], data.followOffset[dataIndex]);
                else
                    newCmd = new Intercept(EntityMgr.inst.entities[data.commandEntIndex[dataIndex]], EntityMgr.inst.entities[0]);

                newCmd.isRunning = data.isRunning[dataIndex];

                newCmd.startPosition = data.startPosition[dataIndex];
                newCmd.commandTime = data.commandTime[dataIndex];
                newCmd.distanceTraveled = data.distanceTraveled[dataIndex];
                newCmd.distanceThreshold = data.distanceThreshold[dataIndex];
                newCmd.timeThreshold = data.timeThreshold[dataIndex];
                if(data.conditionEntIndex[dataIndex] != -1)
                    newCmd.conditionEntity = EntityMgr.inst.entities[data.conditionEntIndex[dataIndex]];
                else
                    newCmd.conditionEntity = null;
                newCmd.conditionEntityType = (EntityType) data.conditionEntityType[dataIndex];

                newCmd.startCondition = (CommandCondition) data.startCondition[dataIndex];
                newCmd.startCommand = data.startCommand[dataIndex];
                newCmd.clearQueueWhenStart = data.clearQueueWhenStart[dataIndex];
                newCmd.insertWhenAdded = data.insertWhenAdded[dataIndex];
                newCmd.startDistanceThreshold = data.startDistanceThreshold[dataIndex];
                if (data.startConditionEntIndex[dataIndex] != -1)
                    newCmd.startConditionEntity = EntityMgr.inst.entities[data.startConditionEntIndex[dataIndex]];
                else
                    newCmd.startConditionEntity = null;
                newCmd.startConditionEntityType = (EntityType) data.startConditionEntityType[dataIndex];

                if (data.commandIndex[dataIndex] != -1)
                {
                    ent.GetComponent<UnitAI>().AddCommand(newCmd);
                    Debug.Log("command #" + dataIndex + " added to " + ent.name);
                    Debug.Log(ent.GetComponent<UnitAI>().commands.Count);
                }
                if (data.startCommand[dataIndex] == true)
                    ent.GetComponent<UnitAI>().startCommands.Add(newCmd);
            }
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

            UnitAI uai = ent.GetComponent<UnitAI>();

            foreach(Command cmd in uai.commands)
            {
                if(cmd is Intercept)
                {
                    Intercept inter = (Intercept)cmd;
                    data.commandType.Add(2);
                    data.followOffset.Add(Vector3.zero);
                    data.followTargetIndex.Add(EntityMgr.inst.entities.IndexOf(inter.targetEntity));
                }
                else if (cmd is Follow)
                {
                    Follow f = (Follow) cmd;
                    data.commandType.Add(1);
                    data.followOffset.Add(f.relativeOffset);
                    data.followTargetIndex.Add(EntityMgr.inst.entities.IndexOf(f.targetEntity));
                }
                else
                {
                    data.commandType.Add(0);
                    data.followOffset.Add(Vector3.zero);
                    data.followTargetIndex.Add(-1);
                }
                    
                data.commandEntIndex.Add(EntityMgr.inst.entities.IndexOf(ent));
                data.commandIndex.Add(uai.commands.IndexOf(cmd));
                data.isRunning.Add(cmd.isRunning);
                data.movePosition.Add(cmd.movePosition);

                data.startPosition.Add(cmd.startPosition);
                data.commandTime.Add(cmd.commandTime);
                data.distanceTraveled.Add(cmd.distanceTraveled);
                data.distanceThreshold.Add(cmd.distanceThreshold);
                data.timeThreshold.Add(cmd.timeThreshold);
                if(cmd.conditionEntity != null)
                    data.conditionEntIndex.Add(EntityMgr.inst.entities.IndexOf(cmd.conditionEntity));
                else
                    data.conditionEntIndex.Add(-1);
                data.conditionEntityType.Add((int) cmd.conditionEntityType);
                data.endCondition.Add((int)cmd.condition);

                data.startCondition.Add((int) cmd.startCondition);
                data.startCommand.Add(cmd.startCommand);
                data.clearQueueWhenStart.Add(cmd.clearQueueWhenStart);
                data.insertWhenAdded.Add(cmd.insertWhenAdded);
                data.startDistanceThreshold.Add(cmd.startDistanceThreshold);
                if (cmd.startConditionEntity != null)
                    data.startConditionEntIndex.Add(EntityMgr.inst.entities.IndexOf(cmd.startConditionEntity));
                else
                    data.startConditionEntIndex.Add(-1);
                data.startConditionEntityType.Add((int) cmd.startConditionEntityType);
            }


            int i = 0;
            foreach(Command cmd in uai.startCommands)
            {
                if (!uai.commands.Contains(cmd))
                {
                    if (cmd is Intercept)
                    {
                        data.commandType.Add(2);
                        data.followOffset.Add(Vector3.zero);
                        data.followTargetIndex.Add(-1);
                    }
                    else if (cmd is Follow)
                    {
                        Follow f = (Follow)cmd;
                        data.commandType.Add(1);
                        data.followOffset.Add(f.relativeOffset);
                        data.followTargetIndex.Add(EntityMgr.inst.entities.IndexOf(f.targetEntity));
                    }
                    else
                    {
                        data.commandType.Add(0);
                        data.followOffset.Add(Vector3.zero);
                        data.followTargetIndex.Add(-1);
                    }

                    data.commandEntIndex.Add(EntityMgr.inst.entities.IndexOf(ent));
                    data.commandIndex.Add(-1);
                    data.isRunning.Add(cmd.isRunning);
                    data.movePosition.Add(cmd.movePosition);

                    data.startPosition.Add(cmd.startPosition);
                    data.commandTime.Add(cmd.commandTime);
                    data.distanceTraveled.Add(cmd.distanceTraveled);
                    data.distanceThreshold.Add(cmd.distanceThreshold);
                    data.timeThreshold.Add(cmd.timeThreshold);
                    if (cmd.conditionEntity != null)
                        data.conditionEntIndex.Add(EntityMgr.inst.entities.IndexOf(cmd.conditionEntity));
                    else
                        data.conditionEntIndex.Add(-1);
                    data.conditionEntityType.Add((int)cmd.conditionEntityType);
                    data.endCondition.Add((int)cmd.condition);

                    data.startCondition.Add((int)cmd.startCondition);
                    data.startCommand.Add(cmd.startCommand);
                    data.clearQueueWhenStart.Add(cmd.clearQueueWhenStart);
                    data.insertWhenAdded.Add(cmd.insertWhenAdded);
                    data.startDistanceThreshold.Add(cmd.startDistanceThreshold);
                    if (cmd.startConditionEntity != null)
                        data.startConditionEntIndex.Add(EntityMgr.inst.entities.IndexOf(cmd.startConditionEntity));
                    else
                        data.startConditionEntIndex.Add(-1);
                    data.startConditionEntityType.Add((int)cmd.startConditionEntityType);
                }
            }
        };
    }
}
