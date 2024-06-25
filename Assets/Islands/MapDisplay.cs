using UnityEngine;
using System.Collections;
using UnityEditor.SearchService;

public class MapDisplay : MonoBehaviour
{

    public Renderer textureRender;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public GameObject[] objects;

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

        MapEmbellishments(meshObj, FindObjectOfType<MapGenerator>().terrainData.uniformScale);
    }

    
    private void MapEmbellishments(Mesh meshObj, float scale)
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
                            GameObject objectToSpawn = objects[Random.Range(0, objects.Length-1)];
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
                    }
                }
            }
            
            lastNoiseHeight = noiseHeight;
            // islandCounter++;
        }
    }


}