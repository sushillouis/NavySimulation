using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRangeLine : MonoBehaviour
{
    public Entity381 ownship;
    public Entity381 target;

    public Command command;
    public Canvas canvas;

    public LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        canvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(ownship != null && target != null)
        {
            lr.SetPosition(0, ownship.position);
            lr.SetPosition(1, target.position);
        }
    }
}
