using UnityEngine;
using UnityEditor;
using System;
using Random = UnityEngine.Random;

public class TreePlacement : EditorWindow
{
    private Texture2D noiseMapTexture;
    //private GameObject[] treePrefabs;
    private GameObject prefab;
    private PlacementGenes genes;

    // Tool editor
    [MenuItem("Tools/Plant Placement")]
    public static void ShowWindow()
    {
        GetWindow<TreePlacement>("Plant Placement");
    }

    private void OnEnable(){
        genes = new PlacementGenes();
        genes.density = 0.5f;
        genes.maxHeight = 100;
        genes.maxSteepness = 25;
    }


    //Sets up the GUI
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        noiseMapTexture = (Texture2D)EditorGUILayout.ObjectField("Noise Map Texture", noiseMapTexture, typeof(Texture2D), false);
        if (GUILayout.Button("Generate Noise"))
        {
            int width = 241;
            int height = 241;
            float scale = 5;
            noiseMapTexture = Noise.GetNoiseMap(width, height, scale);
        }
        EditorGUILayout.EndHorizontal();

        genes.maxHeight = EditorGUILayout.Slider("Max Height", genes.maxHeight, 0, 1000);
        genes.maxSteepness = EditorGUILayout.Slider("Max Steepness", genes.maxSteepness, 0, 90);

        genes.density = EditorGUILayout.Slider("Density", genes.density, 0, 1);

        prefab = (GameObject)EditorGUILayout.ObjectField("Object Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Place objects"))
        {
            PlaceTrees(Terrain.activeTerrain, noiseMapTexture, genes, prefab);
        }
    }

    //Places the trees
    public static void PlaceTrees(Terrain terrain, Texture2D noiseMapTexture, PlacementGenes genes, GameObject prefab)
    {
        Transform parent = new GameObject("PlacedObjects").transform;

        //Iterates in 1m intervals 
        for (int x = 0; x < 241; x++)
        {
            for(int z = 0; z < 241; z++)
            {
                //Gets noise value
                float noiseMapValue = noiseMapTexture.GetPixel(x, z).g;

                //If the value is above threshold, instantiate a plant prefab at this location
                if(Fitness(terrain, noiseMapTexture, genes, x, z) > 1 - genes.density)
                {
                    Vector3 pos = new Vector3(x, 0, z);
                    pos.y = terrain.terrainData.GetInterpolatedHeight(x/241, z / 241);

                    GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                    go.transform.SetParent(parent);
                    //Set to parent so you can delete in one go
                }
            }
        }
    }

    private static float Fitness(Terrain terrain, Texture2D noiseMapTexture, PlacementGenes genes, int x, int z)
    {
        float fitness = noiseMapTexture.GetPixel(x, z).g;

        fitness += Random.Range(-0.25f, 0.25f);

        float steepness = terrain.terrainData.GetSteepness(x / 241, z / 241);
        if (steepness > 36)
        {
            fitness -= 0.7f;
        }

        float height = terrain.terrainData.GetHeight(x, z);
        if (height > 45)
        {
            fitness -= 0.7f;
        }

        return fitness;
    }

    [Serializable]
    public struct PlacementGenes
    {
        public float density;
        public float maxHeight;
        public float maxSteepness;
    }
}
