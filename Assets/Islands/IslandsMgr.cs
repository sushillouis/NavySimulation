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

public enum IslandFormation
{
    Single, Line, Triangle
}

[System.Serializable]
public struct Island
{
    public IslandSize size;
    public int textureChoice;
}

public class IslandsMgr : MonoBehaviour
{
    public AnimationCurve smallIslandCurve;
    public AnimationCurve mediumIslandCurve;
    public AnimationCurve largeIslandCurve;

    public GameObject islandPrefab;
    public GameObject islandPrefab2;
    public GameObject islandPrefab3;

    GameObject declaredPrefab;
    public Transform islandsParent;
    
    public Island[] islandParameters;

    public List<GameObject> islands;
    public int islandCount = 0;

    public TextureChanger textureChanger;
    public IslandFormation formation;
    public int globalIndex;
    
    [HideInInspector]
    public List<Vector3> position;

    [ContextMenu("IslandGenerate")]
    public void ClusterMove()
    {
        RedrawIslands();
    }

    GameObject CreateIsland(IslandSize size, Vector3 position, int texture, int islandIndex)
    {
        textureChanger.UpdateTexture(texture, islandIndex);
        GameObject island = Instantiate(declaredPrefab, position, Quaternion.identity, islandsParent);
        MapGenerator islandGenerator = island.GetComponent<MapGenerator>();
        if(size == IslandSize.Small)
            islandGenerator.terrainData = GenerateTerrainData(IslandSize.Small, smallIslandCurve);
        else if(size == IslandSize.Medium)
            islandGenerator.terrainData = GenerateTerrainData(IslandSize.Medium, mediumIslandCurve);
        else
            islandGenerator.terrainData = GenerateTerrainData(IslandSize.Large, largeIslandCurve);
        islandGenerator.noiseData = GenerateNoiseData();
        islandGenerator.DrawMap();
        islandCount++;

        islands.Add(island);

        return island;
    }

    public void DeclaredPrefab(int currentIsland)
    {
        switch(currentIsland)
        {
            case 0:
                declaredPrefab = islandPrefab;
            break;
            case 1:
                declaredPrefab = islandPrefab2;
            break;
            case 2:
                declaredPrefab = islandPrefab3;
            break;
        }
    }

    public void DeleteAllIslands()
    {
        for (int i = islands.Count - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                Destroy(islands[i]);
                islandCount = 0;
            }
            else
            {
                DestroyImmediate(islands[i]);
                islandCount = 0;
            }
        }
        islands.Clear();
    }

    public void RedrawIslands()
    {
        FormationDeclaration(formation);
        int index = 0;
        foreach (Vector3 pos in position)
        {
            DeclaredPrefab(index);
            CreateIsland(islandParameters[index].size, pos, islandParameters[index].textureChoice, index);
            index++;
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

    TerrainData GenerateTerrainData(IslandSize size, AnimationCurve meshHeightCurve)
    {
        TerrainData terrainData = new TerrainData();
        terrainData.uniformScale = (int)size;
        terrainData.useFlatShading = false;
        terrainData.useFalloff = true;
        terrainData.meshHeightMultiplier = 20;
        terrainData.meshHeightCurve = meshHeightCurve;
        return terrainData;
    }

    void FormationDeclaration(IslandFormation format)
    {
        position.Clear();
        int randomizationPos = Random.Range(-500, 500);
        switch(format)
        {
            case IslandFormation.Single:
                position.Add(new Vector3(5000, 0, 0));
            break;
            case IslandFormation.Line:
                position.Add(new Vector3(5000 + randomizationPos, 0, 0));
                position.Add(new Vector3(-5000, 0, 0));
                position.Add(new Vector3(15000 + randomizationPos, 0, 0));
            break;

            case IslandFormation.Triangle:
                position.Add(new Vector3(5000 + randomizationPos, 0, 0));
                position.Add(new Vector3(-5000, 0, 0));
                position.Add(new Vector3(5000 + randomizationPos, 0, -7000));
            break;

        }

    }
}
