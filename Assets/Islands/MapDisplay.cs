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
    public GameObject[] Trees;
    public GameObject[] rocks;
    public GameObject[] grass;


    private Vector3[] worldArray;

    private float lastNoiseHeight;

    // private IslandsMgr manager;
    // private int islandCounter = 0;

    public void DrawTexture(Texture2D texture)
    {
        textureRender.sharedMaterial.SetTexture("_MainTex", texture);
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();

        meshFilter.transform.localScale = Vector3.one * FindObjectOfType<MapGenerator>().terrainData.uniformScale;

        

        Mesh meshObj = meshData.CreateMesh();

        meshCollider.sharedMesh = meshData.CreateMesh();

        // MapEmbellishments(meshObj, FindObjectOfType<MapGenerator>().terrainData.uniformScale);

       BoundsDecorator(meshObj, FindObjectOfType<MapGenerator>().terrainData.uniformScale);
    }

    
    /*private void MapEmbellishments(Mesh meshObj, float scale)
    {
        Transform parent = new GameObject("PlacedObjects").transform;

        //Loop through vertices 
        for (int i = 0; i < meshObj.vertices.Length; i++)
        {
            //Check vertice's position in game
            Vector3 worldPt = transform.TransformPoint(meshObj.vertices[i] * scale);
            var noiseHeight = worldPt.y;

            //Stop generation if height difference between 2 vertices is too steep
            if(System.Math.Abs(lastNoiseHeight - worldPt.y) < 25)
            {
                //Min height for object generation
                if (noiseHeight > 30)
                {
                    if(scale > 25)
                    {
                        if (Random.Range(1, 25) == 1)
                        {
                            GameObject objectToSpawn = bigIslandObjects[Random.Range(0, bigIslandObjects.Length)];
                            float yTilt = Random.Range(1, 360);
                            Quaternion target = Quaternion.Euler(0, yTilt, 0);
                            GameObject go = Instantiate(objectToSpawn, new Vector3(worldPt.x, worldPt.y, worldPt.z), target);
                            go.transform.SetParent(parent);
                        }
                        else if(Random.Range(1, 8) == 1)
                        {
                            GameObject objectToSpawn = grass[Random.Range(0, grass.Length)];
                            float yTilt = Random.Range(1, 360);
                            Quaternion target = Quaternion.Euler(0, yTilt, 0);
                            GameObject go = Instantiate(objectToSpawn, new Vector3(worldPt.x, worldPt.y, worldPt.z), target);
                            go.transform.SetParent(parent);
                        }
                    }
                    else
                    {
                        //Chance to Generate
                        if (Random.Range(1, 100) == 1)
                        {
                            GameObject objectToSpawn = objects[Random.Range(0, objects.Length)];
                            float yTilt = Random.Range(1, 360);
                            Quaternion target = Quaternion.Euler(0, yTilt, 0);
                            GameObject go = Instantiate(objectToSpawn, new Vector3(worldPt.x, worldPt.y, worldPt.z), target);
                            go.transform.SetParent(parent);
                        }
                        else if(Random.Range(1, 40) == 1)
                        {
                            GameObject objectToSpawn = grass[Random.Range(0, grass.Length)];
                            float yTilt = Random.Range(1, 360);
                            Quaternion target = Quaternion.Euler(0, yTilt, 0);
                            GameObject go = Instantiate(objectToSpawn, new Vector3(worldPt.x, worldPt.y, worldPt.z), target);
                            go.transform.SetParent(parent);
                        }
                    }
                }
            }
            
            lastNoiseHeight = noiseHeight;
            // islandCounter++;
        }
    }*/

    private void BoundsDecorator(Mesh meshObj, float scale)
    {
        Transform parent = new GameObject("PlacedTrees").transform;
        Transform parent2 = new GameObject("PlacedGrass").transform;

        Bounds bounds = meshObj.bounds;
        Vector3 centerPt = transform.TransformPoint(bounds.center * scale);
        Vector3 maxPt = transform.TransformPoint(bounds.max * scale);
        Vector3 minPt = transform.TransformPoint(bounds.min * scale);

        float maxHeight = 0, maxLength = 0, minHeight = 0, minLength = 0;
        int spawncount = 0;

        switch(scale)
        {
            case 50:
            maxHeight = centerPt.x + 2000;
            maxLength = centerPt.z + 2000; 
            minHeight = centerPt.x - 2000;
            minLength = centerPt.z - 2000;
            spawncount = 1500;
            break;
            case 25:
            maxHeight = centerPt.x + 1000;
            maxLength = centerPt.z + 1000; 
            minHeight = centerPt.x - 1000;
            minLength = centerPt.z - 1000;
            spawncount = 700;
            break;
            case 10:
            maxHeight = centerPt.x + 500;
            maxLength = centerPt.z + 500; 
            minHeight = centerPt.x - 500;
            minLength = centerPt.z - 500;
            spawncount = 200;
            break;
        }
        
        centerPt.y = 100;

        for(int i = 0; i <= spawncount; i++)
        {
            float noiseHeight = 0;
            GameObject objectToSpawn = Trees[Random.Range(0, Trees.Length)];
            GameObject objectToSpawn2 = grass[Random.Range(0, grass.Length)];
            Vector3 randomSpawnPosition = new Vector3(Random.Range(minHeight, maxHeight), 1000, Random.Range(minLength, maxLength));
            Vector3 randomSpawnPositionGrass = new Vector3(Random.Range(minPt.x, maxPt.x), 1000, Random.Range(minPt.z, maxPt.z));
            
            if(FindLand(randomSpawnPosition, ref noiseHeight, centerPt) == true)
            {
                float yTilt = Random.Range(1, 360);
                Quaternion target = Quaternion.Euler(0, yTilt, 0);
                GameObject go = Instantiate(objectToSpawn, new Vector3(randomSpawnPosition.x, noiseHeight, randomSpawnPosition.z), target);
                go.transform.SetParent(parent);
            }

            if(FindLand(randomSpawnPositionGrass, ref noiseHeight, centerPt) == true)
            {
                float yTilt = Random.Range(1, 360);
                Quaternion target = Quaternion.Euler(0, yTilt, 0);
                GameObject go2 = Instantiate(objectToSpawn2, new Vector3(randomSpawnPositionGrass.x, noiseHeight, randomSpawnPositionGrass.z), target);
                go2.transform.SetParent(parent2);
            }
        }

    }


    private bool FindLand(Vector3 position, ref float yVal, Vector3 center)
    {
        Ray ray = new Ray(position, Vector3.down);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if(hitInfo.point.y > 10)
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