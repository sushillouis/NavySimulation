using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

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
}

public class IslandsMgr : MonoBehaviour
{
    public AnimationCurve smallIslandCurve;
    public AnimationCurve mediumIslandCurve;
    public AnimationCurve largeIslandCurve;

    public GameObject islandPrefab;
    public Transform islandsParent;

    public Island[] islandParameters;

    public bool autoUpdate;

    public List<GameObject> islands;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Island island in islandParameters)
        {
            CreateIsland(island.size, island.position);
        }
    }

    GameObject CreateIsland(IslandSize size, Vector3 position)
    {
        GameObject island = Instantiate(islandPrefab, position, Quaternion.identity, islandsParent);
        MapGenerator islandGenerator = island.GetComponent<MapGenerator>();
        if(size == IslandSize.Small)
            islandGenerator.terrainData = GenerateTerrainData(size, smallIslandCurve);
        else if(size == IslandSize.Medium)
            islandGenerator.terrainData = GenerateTerrainData(size, mediumIslandCurve);
        else
            islandGenerator.terrainData = GenerateTerrainData(size, largeIslandCurve);
        islandGenerator.noiseData = GenerateNoiseData();
        islandGenerator.DrawMap();

        islands.Add(island);

        return island;
    }

    public void RedrawIslands()
    {
        foreach(GameObject island in islands)
        {
            MapGenerator islandGenerator = island.GetComponent<MapGenerator>();
            islandGenerator.DrawMap();
        }
    }

    NoiseData GenerateNoiseData()
    {
        NoiseData noiseData = new NoiseData();
        noiseData.autoUpdate = true;
        noiseData.normalizeMode = Noise.NormalizeMode.Local;
        noiseData.noiseScale = Random.Range(20f, 100f);
        noiseData.octaves = Random.Range(1, 10);
        noiseData.persistance = Random.Range(0f, 1f);
        noiseData.lacunarity = 1;
        noiseData.seed = Random.Range(-100000, 100000);
        float offsetX = Random.Range(-100000f, 100000f);
        float offsetY = Random.Range(-100000f, 100000f);
        noiseData.offset = new Vector2(offsetY, offsetX);

        Debug.Log(noiseData.noiseScale);
        Debug.Log(noiseData.octaves);
        Debug.Log(noiseData.persistance);
        Debug.Log(noiseData.lacunarity);
        Debug.Log(noiseData.seed);
        Debug.Log(noiseData.offset);

        return noiseData;
    }

    TerrainData GenerateTerrainData(IslandSize size, AnimationCurve meshHeightCurve)
    {
        TerrainData terrainData = new TerrainData();
        terrainData.autoUpdate = true;
        terrainData.uniformScale = (int)size;
        terrainData.useFlatShading = false;
        terrainData.useFalloff = true;
        terrainData.meshHeightMultiplier = 20;
        terrainData.meshHeightCurve = meshHeightCurve;
        return terrainData;
    }
}
