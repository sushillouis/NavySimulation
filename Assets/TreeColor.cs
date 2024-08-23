using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeColor : MonoBehaviour
{
    public Material leafMaterial;
    public Texture2D normalLeaf;
    public Texture2D grassyLeaf;
    public Texture2D dirtLeaf;
    public Texture2D blackSand;
    public Texture2D paintedLeaf;

    public int playerChoice;

    [ContextMenu("UpdateColor")]
    public void UpdateColor()
    {
        UpdateLeafTexture(playerChoice);
    }

    public void UpdateLeafTexture(int playerChoice)
    {
        switch (playerChoice)
        {
            case 0:
                leafMaterial.SetTexture("_TextureA", normalLeaf);
            break;
            case 1:
                leafMaterial.SetTexture("_TextureA", grassyLeaf);
            break;
            case 2:
                leafMaterial.SetTexture("_TextureA", dirtLeaf);
            break;
            case 3:
                leafMaterial.SetTexture("_TextureA", blackSand);
            break;
            case 4:
                leafMaterial.SetTexture("_TextureA", paintedLeaf);
            break;
        }
    }
}
