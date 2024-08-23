using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    public bool FindLand(Vector3 position, ref float yVal, int spawnHeight)
    {
        Ray ray = new Ray(position, Vector3.down);
        Debug.DrawLine(position, position + Vector3.down*1000f, Color.green, 5f);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.point.y >= spawnHeight)
            {
                yVal = hitInfo.point.y;
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }
}
