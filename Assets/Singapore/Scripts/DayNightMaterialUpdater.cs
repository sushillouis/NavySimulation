using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DayNightMaterialUpdater : MonoBehaviour
{
    public Material dayMaterial;
    public Material nightMaterial;
    public GameObject gameObj;
    
    public bool isNightTime = false;
    private bool lastBuildingState = false;
    private void Update()
    {
        if (lastBuildingState != isNightTime)
        {
            lastBuildingState = isNightTime;

            if (isNightTime)
            {
                gameObj.GetComponent<MeshRenderer>().material = nightMaterial;
            }
            else
            {
                gameObj.GetComponent<MeshRenderer>().material = dayMaterial;
            }
            
        }
    }
}
