using UnityEngine;
using UnityEditor;
using System;

public class TreePlacement : EditorWindow
{
    private Texture2D noiseMapTexture;
    //private GameObject[] treePrefabs;
    private float density = 0.5f;
    public float treeNoiseScale = .05f;
    private GameObject prefab;

    [MenuItem("Tools/Plant Placement")]
    public static void ShowWindow()
    {
        GetWindow<TreePlacement>("Plant Placement");
    }


    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        noiseMapTexture = (Texture2D)EditorGUILayout.ObjectField("Noise Map Texture", noiseMapTexture, typeof(Texture2D), false);
        if (GUILayout.Button("Generate Noise"))
        {
            int width = (int)Terrain.activeTerrain.terrainData.size.x;
            int height = (int)Terrain.activeTerrain.terrainData.size.y;
            float scale = 5;
            noiseMapTexture = Noise.GetNoiseMap(width, height, scale);
        }
        EditorGUILayout.EndHorizontal();

        density = EditorGUILayout.Slider("Density", density, 0, 1);

        prefab = (GameObject)EditorGUILayout.ObjectField("Object Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Place objects"))
        {
            PlaceTrees(Terrain.activeTerrain, noiseMapTexture, density, prefab);
        }
    }

    public static void PlaceTrees(Terrain terrain, Texture2D noiseMapTexture, float density, GameObject prefab)
    {
        Transform parent = new GameObject("PlacedObjects").transform;

        for (int x = 0; x < terrain.terrainData.size.x; x++)
        {
            for(int z = 0; z < terrain.terrainData.size.z; z++)
            {
                float noiseMapValue = noiseMapTexture.GetPixel(x, z).g;

                if(noiseMapValue > 1 - density)
                {
                    Vector3 pos = new Vector3(x, 0, z);
                    pos.y = terrain.terrainData.GetInterpolatedHeight(x/terrain.terrainData.size.x, z / (float)terrain.terrainData.size.y);

                    GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                    go.transform.SetParent(parent);
                }
            }
        }
    }
}
