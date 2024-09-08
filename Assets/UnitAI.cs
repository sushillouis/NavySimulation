using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class UnitAI : MonoBehaviour
{
    public Entity381 entity; //public only for ease of debugging
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity381>();
        commands = new List<Command>();
        intercepts = new List<Intercept>();
        moves = new List<Move>();
    }

    public List<Move> moves;
    public List<Command> commands;
    public List<Intercept> intercepts;
    public List<Command> startCommands;
    public bool check;

    // Update is called once per frame
    void Update()
    {
        if (commands.Count > 0) {
            if (commands[0].IsDone()) {
                if (commands.Count > 1)
                    commands[1].Init();
                StopAndRemoveCommand(0);
            } else {
                commands[0].Tick();
                commands[0].isRunning = true;
                DecorateAll();
            }
        }

        CheckStartCommands();
    }

    void StopAndRemoveCommand(int index)
    {
        commands[index].Stop();
        commands.RemoveAt(index);
    }
    
    public void StopAndRemoveAllCommands()
    {
        for(int i = commands.Count - 1; i >= 0; i--) {
            StopAndRemoveCommand(i);
        }
    }

    public void AddCommand(Command c)
    {
        c.Init();
        //print("Adding command; " + c.ToString());
        commands.Add(c);
        if (c is Intercept)
            intercepts.Add(c as Intercept);
        else if (c is Follow)
            ;
        else
            moves.Add(c as Move);
    }

    public void SetCommand(Command c)
    {
        //print("Setting command: " + c.ToString());
        StopAndRemoveAllCommands();
        commands.Clear();
        moves.Clear();
        intercepts.Clear();
        AddCommand(c);

    }

    public void HandleStartCommand(Command c)
    {
        startCommands.Add(c);
        if (c.insertWhenAdded)
            AddCommand(c);
        else
            c.Init();
    }

    public void CheckStartCommands()
    {
        List<Command> triggeredCommands = new List<Command>();
        
        foreach(Command command in startCommands)
        {
            if (command.startCondition == CommandCondition.InRangeOfASpecificEntity)
            {
                if((entity.position - command.startConditionEntity.position).sqrMagnitude < (command.startDistanceThreshold * command.startDistanceThreshold))
                {
                    SetStartCommand(command);
                    triggeredCommands.Add(command);
                }
            }
            else if (command.startCondition == CommandCondition.InRangeOfTypeOfEntity)
            {
                Collider[] colliders = Physics.OverlapSphere(entity.position, command.startDistanceThreshold);

                foreach (Collider collider in colliders)
                {
                    Entity381 target = collider.transform.GetComponent<Entity381>();
                    if (target != null && target != entity && target.entityType == command.startConditionEntityType)
                    {
                        SetStartCommand(command);
                        triggeredCommands.Add(command);
                    }  
                }
            }
        }

        foreach (Command command in triggeredCommands)
        {
            startCommands.Remove(command);
        }
    }

    public void SetStartCommand(Command c)
    {
        if (commands.Contains(c))
            commands.Remove(c);
        if (moves.Contains(c as Move))
            moves.Remove(c as Move);
        if (intercepts.Contains(c as Intercept))
            intercepts.Remove(c as Intercept);

        if (c.clearQueueWhenStart)
            StopAndRemoveAllCommands();

        commands.Insert(0, c);
    }

    //---------------------------------

    public void DecorateAll()
    {
        Command prior = null;
        foreach(Command c in commands) {
            Decorate(prior, c);
            prior = c;
        }
    }

    //decoration logic (UI logic) in general is always convoluted. Ugh
    public void Decorate(Command prior, Command current)
    {
        if (current.line != null) {
            current.line.gameObject.SetActive(entity.isSelected);
            if (prior == null)
                current.line.SetPosition(0, entity.position);
            else
                current.line.SetPosition(0, prior.line.GetPosition(1));

            if (current is Intercept) { //Most specific
                Intercept intercept = current as Intercept;
                if (intercept.isRunning)// 
                    intercept.line.SetPosition(1, intercept.predictedMovePosition);
                else
                    intercept.line.SetPosition(1, intercept.targetEntity.position);
                intercept.line.SetPosition(2, intercept.targetEntity.position);

            } else if (current is Follow) { // Less specific
                Follow f = current as Follow;
                f.line.SetPosition(1, f.targetEntity.position + f.offset);
                f.line.SetPosition(2, f.targetEntity.position);
                //f.line.SetPosition(1, f.predictedMovePosition);
            }
            //Moveposition never changes
        }

        //potential fields lines
        if(!(current is Follow) && !(current is Intercept) && AIMgr.inst.movementType == MovementType.PotentialFields){ 
            Move m = current as Move;
            m.potentialLine.SetPosition(0, entity.position);
            Vector3 newpos = Vector3.zero;
            newpos.x = Mathf.Sin(entity.desiredHeading * Mathf.Deg2Rad) * entity.desiredSpeed;
            newpos.z = Mathf.Cos(entity.desiredHeading * Mathf.Deg2Rad) * entity.desiredSpeed;
            newpos *= 20;
            newpos.y = 1;
            m.potentialLine.SetPosition(1, entity.position + newpos);
            m.potentialLine.gameObject.SetActive(entity.isSelected);
        }


    }

}
