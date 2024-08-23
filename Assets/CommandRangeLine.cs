using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandRangeLine : MonoBehaviour
{
    public Entity381 ownship;
    public Entity381 target;

    public Command command;
    public Canvas canvas;
    public Slider slider;
    public Image sliderHandle;

    public LineRenderer lr;

    bool initialized;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized && command.isRunning)
        {
            Initialize();
        }
        else if(initialized)
        {
            if (!SelectionMgr.inst.selectedEntities.Contains(ownship))
            {
                lr.enabled = false;
                sliderHandle.enabled = false;
            }
            else
            {
                lr.enabled = true;
                sliderHandle.enabled = true;
            }

            if (ownship != null && target != null)
            {
                lr.SetPosition(0, ownship.position);
                lr.SetPosition(1, target.position);
            }

            canvas.transform.position = (ownship.position + target.position) / 2;
            Vector3 normVec = Vector3.Cross(ownship.position - target.position, Vector3.down).normalized;

            canvas.transform.rotation = Quaternion.LookRotation(normVec);
            canvas.transform.eulerAngles = new Vector3(270f, canvas.transform.eulerAngles.y, 0);

            slider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (ownship.position - target.position).magnitude);
            slider.maxValue = (ownship.position - target.position).magnitude;

            command.distanceThreshold = slider.value;

            if (slider.value - (ownship.position - target.position).magnitude < 1)
            {
                command.distanceThreshold += 10;
            }

            if(command.isRunning && ownship.GetComponent<UnitAI>().commands[0] != command)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initialize()
    {
        initialized = true;
        canvas.worldCamera = Camera.main;
        slider.maxValue = (ownship.position - target.position).magnitude;
        slider.value = (ownship.position - target.position).magnitude / 2;
    }

}
