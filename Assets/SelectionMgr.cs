using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMgr : MonoBehaviour
{
    public static SelectionMgr inst;
    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //----------------------------------------------------------------------------------------------------
    public bool isSelecting = false;
    public Vector3 startMousePosition;
    public RectTransform SelectionBoxPanel;
    public RectTransform UICanvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
            SelectNextEntity();

        if (Input.GetMouseButtonDown(0)) { //start box selecting
            isSelecting = true;
            StartBoxSelecting();
        }

        if (Input.GetMouseButtonUp(0)) { //end box selecting
            isSelecting = false;
            EndBoxSelecting();
        }

        if (isSelecting) // while box selecting
            UpdateSelectionBox(startMousePosition, Input.mousePosition);

    }
    void StartBoxSelecting()
    {
        startMousePosition = Input.mousePosition;
        SelectionBoxPanel.gameObject.SetActive(true);
    }
    public float selectionSensitivity = 25;
    void EndBoxSelecting()
    {
        if((Input.mousePosition - startMousePosition).sqrMagnitude > selectionSensitivity)
            ClearSelection(); // if not small box, then clear selection

        SelectEntitiesInBox(startMousePosition, Input.mousePosition);
        SelectionBoxPanel.gameObject.SetActive(false);
    }

    public void UpdateSelectionBox(Vector3 start, Vector3 end)
    {
        SelectionBoxPanel.localPosition = 
            new Vector3(start.x - UICanvas.rect.width/2, start.y - UICanvas.rect.height/2, 0);
        SetPivotAndAnchors(start, end);
        SelectionBoxPanel.sizeDelta = new Vector2(Mathf.Abs(end.x - start.x), Mathf.Abs(start.y - end.y));
    }
    public Vector2 anchorMin = Vector2.up;
    public Vector2 anchorMax = Vector2.up;
    public Vector3 pivot = Vector2.up;
    public void SetPivotAndAnchors(Vector3 start, Vector3 end)
    {
        Vector3 diff = end - start;
        // which quadrant?
        if(diff.x >= 0 && diff.y >= 0) {//q1
            SetPAValues(Vector2.zero);
        } else if (diff.x < 0 && diff.y >= 0) { //q2
            SetPAValues(Vector2.right);
        } else if (diff.x < 0 && diff.y < 0) { //q3
            SetPAValues(Vector2.one);
        } else { //q4
            SetPAValues(Vector2.up);
        }
    }
    void SetPAValues(Vector2 val)
    {
        SelectionBoxPanel.anchorMax = val;
        SelectionBoxPanel.anchorMin = val;
        SelectionBoxPanel.pivot = val;
    }
    //--------------------------------------------------------------
    public Vector3 wp1;
    public Vector3 wp2;
    public void SelectEntitiesInBox(Vector3 start, Vector3 end)
    {
        wp1 = Camera.main.ScreenToViewportPoint(start);
        wp2 = Camera.main.ScreenToViewportPoint(end);
        Vector3 min = Vector3.Min(wp1, wp2);
        Vector3 max = Vector3.Max(wp1, wp2);
        min.z = Camera.main.nearClipPlane;
        max.z = Camera.main.farClipPlane;
        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        foreach(Entity381 ent in EntityMgr.inst.entities) 
            if (bounds.Contains(Camera.main.WorldToViewportPoint(ent.transform.localPosition))) 
                SelectEntity(ent, shouldClearSelection: false);

    }
    //----------------------------------------------------------------------------------------------------

    public int selectedEntityIndex = -1;
    public Entity381 selectedEntity = null;
    public List<Entity381> selectedEntities = new List<Entity381>();

    public void SelectNextEntity()
    {
        selectedEntityIndex = 
            (selectedEntityIndex >= EntityMgr.inst.entities.Count - 1 ? 0 : selectedEntityIndex + 1);
        SelectEntity(EntityMgr.inst.entities[selectedEntityIndex], 
            shouldClearSelection: !Input.GetKey(KeyCode.LeftShift));
    }

    public void ClearSelection()
    {
        foreach (Entity381 ent in EntityMgr.inst.entities)
            ent.isSelected = false;
        selectedEntities.Clear();
    }

    public void SelectEntity(Entity381 ent, bool shouldClearSelection = true)
    {
        if (ent != null && (selectedEntityIndex = EntityMgr.inst.entities.FindIndex(x => (x == ent))) >= 0) {
            if (shouldClearSelection) 
                ClearSelection();

            selectedEntity = ent;
            selectedEntity.isSelected = true;
            selectedEntities.Add(ent);
        }
    }

}
