using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRSelectionMgr : MonoBehaviour
{
    public LineRenderer rightRay;
    [Range(50f, 1000f)]
    public float selectionRadius;
    public GameObject selectionCircle;
    public Material selectionMaterial;
    public Material moveMaterial;
    public Material followMaterial;
    public Material interceptMaterial;

    public static VRSelectionMgr inst;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 selectionCenter = rightRay.GetPosition(1);

        if (!VRControlMgr.inst.rightGripPress.action.IsPressed())
        {
            if (VRControlMgr.inst.rightTriggerPress.action.IsPressed())
            {
                selectionRadius += VRControlMgr.inst.rightThumbstickMove.action.ReadValue<Vector2>().y * 50;
                selectionRadius = Mathf.Clamp(selectionRadius, 50, 1000);

                selectionCircle.SetActive(true);
                selectionCircle.transform.localScale = new Vector3(2 * selectionRadius, 0.1f, 2 * selectionRadius);
                selectionCircle.transform.position = selectionCenter;
                selectionCircle.GetComponent<Renderer>().material = selectionMaterial;

                Collider[] hitColliders = Physics.OverlapSphere(selectionCenter, selectionRadius);
                SelectionMgr.inst.ClearSelection();
                foreach (Collider collider in hitColliders)
                {
                    if (collider.gameObject.GetComponent<Entity381>() != null)
                        SelectionMgr.inst.SelectEntity(collider.gameObject.GetComponent<Entity381>(), shouldClearSelection: false);
                }
            }
            else
                selectionCircle.SetActive(false);
        }
        else
        {
            float rClickRadius = Mathf.Sqrt(AIMgr.inst.rClickRadiusSq);
            selectionCircle.SetActive(true);
            selectionCircle.transform.localScale = new Vector3(2 * rClickRadius, 0.1f, 2 * rClickRadius);
            selectionCircle.transform.position = selectionCenter;
            Entity381 ent = AIMgr.inst.FindClosestEntInRadius(selectionCenter, AIMgr.inst.rClickRadiusSq);

            if(VRAIMgr.inst.followStep != CommandSteps.finished)
                selectionCircle.GetComponent<Renderer>().material = followMaterial;
            else if (ent == null)
                selectionCircle.GetComponent<Renderer>().material = moveMaterial;
            else
            {
                if (VRControlMgr.inst.AButtonPress.action.IsPressed())
                    selectionCircle.GetComponent<Renderer>().material = interceptMaterial;
                else
                    selectionCircle.GetComponent<Renderer>().material = followMaterial;
            }
        }

    }
}
