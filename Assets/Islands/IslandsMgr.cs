using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum IslandSize
{
    Small = 10,
    Medium = 25,
    Large = 50,
}

[System.Serializable]
public struct Island
{
    public IslandSize size;
    public Vector3 position;
    public GameObject[] trees;
    public int treeClusters;    //How many tree groups will spawn
    public int treeCount;   //How many trees spawn per group
}

public class IslandsMgr : MonoBehaviour
{
    public AnimationCurve smallIslandCurve;
    public AnimationCurve mediumIslandCurve;
    public AnimationCurve largeIslandCurve;

    public GameObject islandPrefab;
    public Transform islandsParent;
    
    public Island[] islandParameters;

    public List<GameObject> islands;
    public int islandCount = 0;
    public int treeClustersDropdown = 0;
    public float treeDensitySlider = 0;

    public int islandSizeMenu = 2;

    GameObject CreateIsland(int size, Vector3 position, GameObject[] islandTrees, int treeClusters, float treeCount)
    {
        // print(islandSizeMenu);
        // islandSizeMenu = 2;
        GameObject island = Instantiate(islandPrefab, position, Quaternion.identity, islandsParent);
        MapGenerator islandGenerator = island.GetComponent<MapGenerator>();
        if(islandSizeMenu == 0)
            islandGenerator.terrainData = GenerateTerrainData(IslandSize.Small, smallIslandCurve, islandTrees, treeClusters, treeCount);
        else if(islandSizeMenu == 1)
            islandGenerator.terrainData = GenerateTerrainData(IslandSize.Medium, mediumIslandCurve, islandTrees, treeClusters, treeCount);
        else
            islandGenerator.terrainData = GenerateTerrainData(IslandSize.Large, largeIslandCurve, islandTrees, treeClusters, treeCount);
        islandGenerator.noiseData = GenerateNoiseData();
        islandGenerator.DrawMap();
        islandCount++;

        islands.Add(island);

        return island;
    }

    public void DeleteAllIslands()
    {
        for (int i = islands.Count - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                Destroy(islands[i]);
                Destroy(GameObject.Find("PlacedObjs"+ i));
                islandCount = 0;
            }
            else
            {
                DestroyImmediate(islands[i]);
                DestroyImmediate(GameObject.Find("PlacedObjs"+i));
                islandCount = 0;
            }
        }
        islands.Clear();
    }

    public void RedrawIslands()
    {
        DeleteAllIslands();

        foreach (Island island in islandParameters)
        {
            CreateIsland(islandSizeMenu, island.position, island.trees, island.treeClusters, island.treeCount);
        }

    }

    public void RedrawButtonIslands()
    {
        DeleteAllIslands();

        foreach (Island island in islandParameters)
        {
            CreateIsland(islandSizeMenu, island.position, island.trees, treeClustersDropdown, treeDensitySlider);
        }

    }

    NoiseData GenerateNoiseData()
    {
        NoiseData noiseData = new NoiseData();
        noiseData.normalizeMode = Noise.NormalizeMode.Local;
        noiseData.noiseScale = Random.Range(20f, 100f);
        noiseData.octaves = Random.Range(1, 10);
        noiseData.persistance = Random.Range(0f, 1f);
        noiseData.lacunarity = 1;
        noiseData.seed = Random.Range(-100000, 100000);
        float offsetX = Random.Range(-100000f, 100000f);
        float offsetY = Random.Range(-100000f, 100000f);
        noiseData.offset = new Vector2(offsetY, offsetX);

        return noiseData;
    }

    TerrainData GenerateTerrainData(IslandSize size, AnimationCurve meshHeightCurve, GameObject[] currentTrees, int treeClusters, float treeCount)
    {
        TerrainData terrainData = new TerrainData();
        terrainData.uniformScale = (int)size;
        terrainData.useFlatShading = false;
        terrainData.useFalloff = true;
        terrainData.meshHeightMultiplier = 20;
        terrainData.meshHeightCurve = meshHeightCurve;
        terrainData.treeHolder = currentTrees;
        terrainData.treeCluster = treeClusters;
        terrainData.treeDensity = treeCount;
        return terrainData;
    }
}
