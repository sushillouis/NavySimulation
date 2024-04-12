using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class MaterialVisibilityUpdater : MonoBehaviour {

    public Material defaultMaterial;
    public Material invisibleMaterial;
    public GameObject gameObj;
    

    public bool isVisible = true;
    private bool lastState;

    private void Update()
    {
        if (lastState != isVisible)
        {
            lastState = isVisible;

            if (isVisible)
            {
                gameObj.GetComponent<MeshRenderer>().material = defaultMaterial;
            }
            else
            {
                MeshRenderer mr = gameObj.GetComponent<MeshRenderer>();
                defaultMaterial = mr.material;
                mr.material = invisibleMaterial;
            }
            
        }
    }
}
