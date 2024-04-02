using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMgr : MonoBehaviour
{
    public static ControlMgr inst;
    private GameInputs input;
    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        input = new GameInputs();
        input.Enable();
    }

    public float deltaSpeed = 1;
    public float deltaHeading = 2;
    private float speedChange;
    private float headingChange;

    // Update is called once per frame
    void Update()
    {
        if (SelectionMgr.inst.selectedEntity != null)
        {
            speedChange = input.Entities.Speed.ReadValue<float>();
            headingChange = input.Entities.Heading.ReadValue<float>();

            if (input.Entities.Speed.triggered)
            {
                SelectionMgr.inst.selectedEntity.desiredSpeed += speedChange * deltaSpeed;
                SelectionMgr.inst.selectedEntity.desiredSpeed =
                    Utils.Clamp(SelectionMgr.inst.selectedEntity.desiredSpeed, SelectionMgr.inst.selectedEntity.minSpeed, SelectionMgr.inst.selectedEntity.maxSpeed);
            }

            if (input.Entities.Heading.triggered)
            {
                SelectionMgr.inst.selectedEntity.desiredHeading += headingChange * deltaHeading;
                SelectionMgr.inst.selectedEntity.desiredHeading = Utils.Degrees360(SelectionMgr.inst.selectedEntity.desiredHeading);
            }
        }
    }

    
    //------------------------------------------
}
