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
    private GameObject[] Trees;
    public GameObject[] rocks;
    public GameObject[] grass;

    private int clusterCount;


    public void DrawTexture(Texture2D texture)
    {
        textureRender.sharedMaterial.SetTexture("_MainTex", texture);
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData)
    {
        Trees = FindObjectOfType<MapGenerator>().terrainData.treeHolder;

        meshFilter.sharedMesh = meshData.CreateMesh();

        meshFilter.transform.localScale = Vector3.one * FindObjectOfType<MapGenerator>().terrainData.uniformScale;        

        Mesh meshObj = meshData.CreateMesh();

        meshCollider.sharedMesh = meshData.CreateMesh();

       BoundsDecorator(meshObj, FindObjectOfType<MapGenerator>().terrainData.uniformScale);
    }

    private void BoundsDecorator(Mesh meshObj, float scale)
    {
        Transform parent = new GameObject("PlacedObjs" + FindObjectOfType<IslandsMgr>().islandCount).transform;
        parent.transform.SetParent(FindObjectOfType<IslandsMgr>().islandsParent);
        clusterCount = FindObjectOfType<MapGenerator>().terrainData.treeCluster;
        //Transform parent2 = new GameObject("PlacedGrass").transform;

        Bounds bounds = meshObj.bounds;
        Vector3 centerPt = transform.TransformPoint(bounds.center * scale);
        Vector3 maxPt = transform.TransformPoint(bounds.max * scale);
        Vector3 minPt = transform.TransformPoint(bounds.min * scale);

        float maxHeight , maxLength, minHeight, minLength, noiseRef = 0;
        float spawncount = FindObjectOfType<MapGenerator>().terrainData.treeDensity;
        int spawnHeight = 40;

        switch(scale)
        {
            case 50:
            spawnHeight = 180;
            for(int i = 0; i < clusterCount; i++)
            {
                Vector3 randomSpawnPos = new Vector3(Random.Range(minPt.x, maxPt.x), 1000, Random.Range(minPt.z, maxPt.z));
                do{
                    randomSpawnPos = new Vector3(Random.Range(minPt.x, maxPt.x), 1000, Random.Range(minPt.z, maxPt.z));
                }while(FindLand(randomSpawnPos, ref noiseRef, spawnHeight) == false);
                maxHeight = randomSpawnPos.x + 350;
                maxLength = randomSpawnPos.z + 350; 
                minHeight = randomSpawnPos.x - 350;
                minLength = randomSpawnPos.z - 350;
                spawnTrees(centerPt, parent, spawncount, minHeight, maxHeight, minLength, maxLength, spawnHeight);
            }
            break;
            case 25:
            spawnHeight = 70;
            for(int i = 0; i < clusterCount; i++)
            {
                Vector3 randomSpawnPos = new Vector3(Random.Range(minPt.x, maxPt.x), 1000, Random.Range(minPt.z, maxPt.z));
                do{
                    randomSpawnPos = new Vector3(Random.Range(minPt.x, maxPt.x), 1000, Random.Range(minPt.z, maxPt.z));
                }while(FindLand(randomSpawnPos, ref noiseRef, spawnHeight) == false);
                maxHeight = randomSpawnPos.x + 200;
                maxLength = randomSpawnPos.z + 200; 
                minHeight = randomSpawnPos.x - 200;
                minLength = randomSpawnPos.z - 200;
                spawnTrees(centerPt, parent, spawncount, minHeight, maxHeight, minLength, maxLength, spawnHeight);
            }
            break;
            case 10:
            for(int i = 0; i < clusterCount; i++)
            {
                Vector3 randomSpawnPos = new Vector3(Random.Range(minPt.x, maxPt.x), 1000, Random.Range(minPt.z, maxPt.z));
                do{
                    randomSpawnPos = new Vector3(Random.Range(minPt.x, maxPt.x), 1000, Random.Range(minPt.z, maxPt.z));
                }while(FindLand(randomSpawnPos, ref noiseRef, spawnHeight) == false);
                maxHeight = randomSpawnPos.x + 100;
                maxLength = randomSpawnPos.z + 100; 
                minHeight = randomSpawnPos.x - 100;
                minLength = randomSpawnPos.z - 100;
                spawnTrees(centerPt, parent, spawncount, minHeight, maxHeight, minLength, maxLength, spawnHeight);
            }
            break;
        }
        
        centerPt.y = 100;
    }

    private void spawnTrees(Vector3 centerPoint, Transform parent, float spawncount, float minHeight, float maxHeight, float minLength, float maxLength, int spawnHeight)
    {
        for(int i = 0; i <= spawncount; i++)
        {
            float noiseHeight = 0;
            GameObject objectToSpawn = Trees[Random.Range(0, Trees.Length)];
            Vector3 randomSpawnPosition = new Vector3(Random.Range(minHeight, maxHeight), 1000, Random.Range(minLength, maxLength));

            if(FindLand(randomSpawnPosition, ref noiseHeight, spawnHeight) == true)
            {
                float yTilt = Random.Range(1, 360);
                Quaternion target = Quaternion.Euler(0, yTilt, 0);
                GameObject go = Instantiate(objectToSpawn, new Vector3(randomSpawnPosition.x, noiseHeight - 20, randomSpawnPosition.z), target);
                go.transform.SetParent(parent);
            }
    }
    }


    private bool FindLand(Vector3 position, ref float yVal, int spawnHeight)
    {
        Ray ray = new Ray(position, Vector3.down);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.point.y > spawnHeight)
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