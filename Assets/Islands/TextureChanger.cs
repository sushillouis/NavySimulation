using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChanger : MonoBehaviour
{
    public Material baseMat1;
    public Material baseMat2;
    public Material baseMat3;

    public Texture2D Grassy;
    public Texture2D VeryGrassy;
    public Texture2D DirtGrass;
    public Texture2D BlackSand;
    public Texture2D PaintedGrass;

    public Texture2D Sand;
    public Texture2D GrassySand;
    public Texture2D VeryGrassySand;
    public Texture2D DirtSand;
    public Texture2D BlackSandy;
    public Texture2D PaintedSand;


    public IslandsMgr islandMgr;
    public TreeMaker treeMaker;

    Material baseMat;

    private float scale1 = .14f;
    private float scale3 = .07f;
    private float scale4 = .05f;

    public int playerChoice;

    [ContextMenu("UpdateLandColor")]
    public void UpdateLandColor()
    {
        // UpdateTexture(playerChoice);
    }

    public void UpdateTexture(int textureChoice, int islandIndex)
    {
        switch(islandIndex)
        {
            case 0:
                baseMat = baseMat1;
            break;
            case 1:
                baseMat = baseMat2;
            break;
            case 2:
                baseMat = baseMat3;
            break;

        }
        switch (textureChoice)
        {
            case 0:
            if(islandMgr.islandParameters[treeMaker.islandChoice].size != IslandSize.Small)
                baseMat.SetTexture("_TextureA", GrassySand);
            else
                baseMat.SetTexture("_TextureA", Sand);
                baseMat.SetTexture("_TextureB", Grassy);
                baseMat.SetFloat("_Scale", scale1);
            break;
            case 1:
            if(islandMgr.islandParameters[treeMaker.islandChoice].size != IslandSize.Small)
                baseMat.SetTexture("_TextureA", VeryGrassySand);
            else
                baseMat.SetTexture("_TextureA", Sand);
                baseMat.SetTexture("_TextureB", VeryGrassy);
                baseMat.SetFloat("_Scale", scale4);
            break;
            case 2:
            if(islandMgr.islandParameters[treeMaker.islandChoice].size != IslandSize.Small)
                baseMat.SetTexture("_TextureA", DirtSand);
            else
                baseMat.SetTexture("_TextureA", Sand);
                baseMat.SetTexture("_TextureB", DirtGrass);
                baseMat.SetFloat("_Scale", scale3);
            break;
            case 3:
            if(islandMgr.islandParameters[treeMaker.islandChoice].size != IslandSize.Small)
                baseMat.SetTexture("_TextureA", BlackSandy);
            else
                baseMat.SetTexture("_TextureA", BlackSand);
                baseMat.SetTexture("_TextureB", DirtGrass);
                baseMat.SetFloat("_Scale", scale3);
            break;
            case 4:
            if(islandMgr.islandParameters[treeMaker.islandChoice].size != IslandSize.Small)
                baseMat.SetTexture("_TextureA", PaintedSand);
            else
                baseMat.SetTexture("_TextureA", Sand);
                baseMat.SetTexture("_TextureB", PaintedGrass);
                baseMat.SetFloat("_Scale", scale1);
            break;
        }
    }
}