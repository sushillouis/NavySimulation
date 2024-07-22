using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionMode{
    SelectionBox,
    SelectionCircle
}

public class VRSelectionMgr : MonoBehaviour
{
    public LineRenderer rightRay;

    [Range(50f, 1000f)]
    public float selectionRadius;
    public SelectionMode selectionMode;

    public GameObject selectionCircle;
    public GameObject selectionCube;

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
        Vector3 pointerPos = rightRay.GetPosition(1);

        if(selectionMode == SelectionMode.SelectionBox)
        {
            if (VRControlMgr.inst.rightGripPress.action.IsPressed())
            {
                CommandCircle(pointerPos);
            }
            else
            {
                selectionCircle.SetActive(false);
                if (selectionBoxStep != CommandSteps.finished)
                {
                    SelectionBox(pointerPos);
                }
                else if (VRControlMgr.inst.rightTriggerPress.action.IsPressed())
                {
                    selectionBoxStep = CommandSteps.started;
                }
            }
                
        }
        else
        {
            if (VRControlMgr.inst.rightGripPress.action.IsPressed())
                CommandCircle(pointerPos);
            else
                SelectionCircle(pointerPos);
        }
    }

    void SelectionCircle(Vector3 selectionCenter)
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

    void CommandCircle(Vector3 selectionCenter)
    {
        float rClickRadius = Mathf.Sqrt(AIMgr.inst.rClickRadiusSq);
        selectionCircle.SetActive(true);
        selectionCircle.transform.localScale = new Vector3(2 * rClickRadius, 0.1f, 2 * rClickRadius);
        selectionCircle.transform.position = selectionCenter;
        Entity381 ent = AIMgr.inst.FindClosestEntInRadius(selectionCenter, AIMgr.inst.rClickRadiusSq);

        if (VRAIMgr.inst.followStep != CommandSteps.finished)
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

    CommandSteps selectionBoxStep = CommandSteps.finished;
    Vector3 selectionBoxStartPos;
    void SelectionBox(Vector3 cursorPos)
    {
        if (selectionBoxStep == CommandSteps.started)
        {
            selectionCube.SetActive(true);
            selectionBoxStartPos = cursorPos;
            selectionBoxStep = CommandSteps.selecting;
        }

        Vector3 boxCenter = Vector3.zero;
        Vector3 boxScale = Vector3.zero;;
        float width = Mathf.Abs(cursorPos.x - selectionBoxStartPos.x);
        float height = Mathf.Abs(cursorPos.z - selectionBoxStartPos.z);

        if (selectionBoxStep == CommandSteps.selecting)
        {
            float centerX;
            float centerZ;

            if(cursorPos.x > selectionBoxStartPos.x)
                centerX = selectionBoxStartPos.x + width/2;  
            else
                centerX = selectionBoxStartPos.x - width/2; 

            if(cursorPos.z > selectionBoxStartPos.z)
                centerZ = selectionBoxStartPos.z + height / 2;
            else
                centerZ = selectionBoxStartPos.z - height / 2;

            boxCenter = new Vector3(centerX, 0, centerZ);
            boxScale = new Vector3(width, 1, height);
            selectionCube.transform.position = boxCenter;
            selectionCube.transform.localScale = boxScale;
        }

        if (!VRControlMgr.inst.rightTriggerPress.action.IsPressed() && selectionBoxStep == CommandSteps.selecting)
        {
            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxScale / 2);
            SelectionMgr.inst.ClearSelection();
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.GetComponent<Entity381>() != null)
                    SelectionMgr.inst.SelectEntity(collider.gameObject.GetComponent<Entity381>(), shouldClearSelection: false);
            }
            selectionBoxStep = CommandSteps.finished;
            selectionCube.SetActive(false); 
        }
    }
}
