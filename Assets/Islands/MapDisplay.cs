using UnityEngine;
using System.Collections;
using UnityEditor.SearchService;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using System;
using UnityEditor;
using System.Linq;

public class MapDisplay : MonoBehaviour
{

    public Renderer textureRender;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;
    public Mesh meshObj;

    public void DrawTexture(Texture2D texture)
    {
        textureRender.sharedMaterial.SetTexture("_MainTex", texture);
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();

        meshFilter.transform.localScale = Vector3.one * FindObjectOfType<MapGenerator>().terrainData.uniformScale;        

        meshObj = meshData.CreateMesh();

        meshCollider.sharedMesh = meshData.CreateMesh();
    }
}