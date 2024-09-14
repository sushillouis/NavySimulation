﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMgr : MonoBehaviour
{
    public static LineMgr inst;

    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lines.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LineRenderer MovePrefab;
    public LineRenderer FollowPrefab;
    public LineRenderer InterceptPrefab;
    public LineRenderer PotentialPrefab;
    public LineRenderer VOPrefab;
    public LineRenderer CommandRangePrefab;

    public List<LineRenderer> lines = new List<LineRenderer>();
    public LineRenderer CreateMoveLine(Vector3 p1, Vector3 p2)
    {
        LineRenderer lr = Instantiate<LineRenderer>(MovePrefab, transform);
        lr.SetPosition(0, p1);
        lr.SetPosition(1, p2);
        lines.Add(lr);
        return lr;
    }

    public LineRenderer CreateVOLine(Vector3 p1, Vector3 p2)
    {
        LineRenderer lr = Instantiate<LineRenderer>(VOPrefab, transform);
        lr.SetPosition(0, p1);
        lr.SetPosition(1, p2);
        lines.Add(lr);
        return lr;
    }

    public LineRenderer CreatePotentialLine(Vector3 p1)
    {
        LineRenderer lr = Instantiate<LineRenderer>(PotentialPrefab, transform);
        lr.SetPosition(0, p1);
        lr.SetPosition(1, Vector3.zero);
        lines.Add(lr);
        return lr;
    }

    public LineRenderer CreateFollowLine(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        LineRenderer lr = Instantiate<LineRenderer>(FollowPrefab, transform);
        lr.SetPosition(0, p1);
        lr.SetPosition(1, p2);
        lr.SetPosition(2, p3);
        lines.Add(lr);
        return lr;
    }

    public LineRenderer CreateInterceptLine(Vector3 p1, Vector3 p2, Vector2 p3)
    {
        LineRenderer lr = Instantiate<LineRenderer>(InterceptPrefab, transform);
        lr.SetPosition(0, p1);
        lr.SetPosition(1, p2);
        lr.SetPosition(2, p3);
        lines.Add(lr);
        return lr;
    }

    public LineRenderer CreateCommandRangeLine(Entity381 ownship, Entity381 target)
    {
        LineRenderer lr = Instantiate(CommandRangePrefab, transform);
        lr.SetPosition(0, ownship.position);
        lr.SetPosition(1, target.position);
        lines.Add(lr);
        return lr;
    }

    public LineRenderer tmp;
    public void DestroyLR(LineRenderer lr)
    {
        tmp = null;
        if (lines.Contains(lr)) {
            tmp = lr;
            lines.Remove(lr);
        }
        Destroy(lr);

    }

    public void DestroyAllLines()
    {
        foreach(Transform child in this.transform)
        {
            Debug.Log("Destroy called");
            Destroy(child.gameObject);
        }

        lines.Clear();
    }
}
