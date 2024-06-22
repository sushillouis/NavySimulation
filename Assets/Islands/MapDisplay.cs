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

        MapEmbellishments(meshObj);
    }


    // Fenn Notes:
    // Objs in the main prefabs folder
    
    private void MapEmbellishments(Mesh meshObj)
    {
        Transform parent = new GameObject("PlacedObjects").transform;

        //Loop through vertices 
        for (int i = 0; i < meshObj.vertices.Length; i++)
        {
            //Check vertice's position in game
            Vector3 worldPt = transform.TransformPoint(meshObj.vertices[i]);
            var noiseHeight = worldPt.y;

            //Stop generation if height difference between 2 vertices is too steep
            if(System.Math.Abs(lastNoiseHeight - worldPt.y) < 25)
            {
                //Min height for object generation
                if (noiseHeight > 10)
                {
                    //Chance to Generate
                    if (Random.Range(1, 5) == 1)
                    {
                        GameObject objectToSpawn = objects[Random.Range(0, objects.Length)];
                        var spawnAboveTerrainBy = noiseHeight * 2;
                        GameObject go = Instantiate(objectToSpawn, new Vector3(worldPt.x, spawnAboveTerrainBy, worldPt.z), Quaternion.identity);
                        go.transform.SetParent(parent);
                    }
                }
            }
            
            lastNoiseHeight = noiseHeight;
        }
    }


}