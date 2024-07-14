using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSelectionMgr : MonoBehaviour
{
    public LineRenderer rightRay;
    public float selectionRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!VRControlMgr.inst.rightGripPress.action.IsPressed())
        {
            if (VRControlMgr.inst.rightTriggerPress.action.IsPressed())
            {
                Collider[] hitColliders = Physics.OverlapSphere(rightRay.GetPosition(1), selectionRadius);
                SelectionMgr.inst.ClearSelection();
                foreach (Collider collider in hitColliders)
                {
                    if (collider.gameObject.GetComponent<Entity381>() != null)
                        SelectionMgr.inst.SelectEntity(collider.gameObject.GetComponent<Entity381>(), shouldClearSelection: false);
                }
            }
        }
            
    }
}
