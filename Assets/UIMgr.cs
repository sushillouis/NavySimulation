using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    public static UIMgr inst;
    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public Text entityName;
    public Text speed;
    public Text desiredSpeed;
    public Text heading;
    public Text desiredHeading;

    // Update is called once per frame
    void Update()
    {
        if(SelectionMgr.inst.selectedEntity != null) {
            Entity381 ent = SelectionMgr.inst.selectedEntity;
            entityName.text = ent.gameObject.name;
            speed.text = ent.speed.ToString("F2") + " m/s";
            desiredSpeed.text = ent.desiredSpeed.ToString("F2") + " m/s";
            heading.text = ent.heading.ToString("F1") + " deg";
            desiredHeading.text = ent.desiredHeading.ToString("F1") + " deg";
        }
    }
}
