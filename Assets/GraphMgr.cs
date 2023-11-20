﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphMgr : MonoBehaviour
{
    public static GraphMgr inst;
    public GameObject graph;
    [Min(1f)]
    public float maxMag = 50;
    public Vector2 size;
    public Vector3 position;
    public int resolution;

    private void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (SelectionMgr.inst.selectedEntity != null)
                CreateGraph(SelectionMgr.inst.selectedEntity);
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            DeleteAllGraphs();
        }
    }

    public void DeleteAllGraphs()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Graph");
        foreach (GameObject go in gos)
            Destroy(go);
    }

    public void CreateGraph(Entity381 entity)
    {
        GameObject g = Instantiate(graph, entity.transform);
        g.transform.localPosition = new Vector3(position.x, 0, position.z);
        g.GetComponent<GraphPlane>().entity = entity;
    }
}
