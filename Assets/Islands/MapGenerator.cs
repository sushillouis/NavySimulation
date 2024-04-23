using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct TerrainTypes
{
    public string name;
    public float height;
    public Color color;
}

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {Noise, Color, Mesh};

    public DrawMode drawMode;

    public int width;
    public int height;
    public float scale;
    public int octaves;
    [Range(0f, 1f)]
    public float persistance;
    public float lacrunarity;
    public int seed;
    public Vector2 offset;

    public Renderer render;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public bool autoUpdate;

    public TerrainTypes[] regions;

    float[,] noiseMap;

    public void GenerateMap()
    {
        CreateNoiseMap(width, height, scale, seed, octaves, persistance, lacrunarity, offset);

        if (drawMode == DrawMode.Noise)
            DrawNoiseMap();
        else if (drawMode == DrawMode.Color)
            DrawColorMap();
        else if (drawMode == DrawMode.Mesh)
            DrawMesh();
    }

    public void DrawNoiseMap()
    {
        Texture2D texture = new Texture2D(width, height);

        Color[] colors = new Color[width * height];
        for(int i = 0; i < width; i++) { 
            for(int j = 0; j < height; j++)
            {
                colors[j * width + i] = Color.Lerp(Color.black, Color.white, noiseMap[i, j]);
            }
        }
        DrawTexture(texture, colors);
    }

    public void DrawColorMap()
    {
        Texture2D texture = new Texture2D(width, height);

        Color[] colors = new Color[width * height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float currentHeight = noiseMap[i, j];
                for (int k = 0; k < regions.Length; k++)
                {
                    if (currentHeight < regions[k].height)
                    {
                        colors[j * width + i] = regions[k].color;
                        break;
                    }

                }
            }
        }
        DrawTexture(texture, colors);
    }

    public void DrawTexture(Texture2D texture, Color[] colors)
    {
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colors);
        texture.Apply();
        render.sharedMaterial.SetTexture("_MainTex", texture);
        render.transform.localScale = new Vector3(width, 1, height);
    }

    public void CreateNoiseMap(int width, int height, float scale, int seed, int octaves, float peristance, float lacunarity, Vector2 offset)
    {
        noiseMap = new float[width, height];

        System.Random rand = new System.Random(seed);
        Vector2[] offsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++)
        {
            float x = rand.Next(-100000, 100000) + offset.x;
            float y = rand.Next(-100000, 100000) + offset.y;
            offsets[i] = new Vector2(x, y);
        }

        if (scale <= 0)
            scale = 0.0001f;

        float max = float.MinValue;
        float min = float.MaxValue;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int k = 0; k < octaves; k++)
                {
                    float x = (i - halfWidth) / scale * frequency + offsets[k].x;
                    float y = (j - halfHeight) / scale * frequency + offsets[k].y;

                    float perl = Mathf.PerlinNoise(x, y) * 2 - 1;
                    noiseHeight += perl * amplitude;

                    amplitude *= peristance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > max)
                    max = noiseHeight;
                else if (noiseHeight < min)
                    min = noiseHeight;
                noiseMap[i, j] = noiseHeight;
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                noiseMap[i, j] = Mathf.InverseLerp(min, max, noiseMap[i, j]);
            }
        }
    }

    public MeshData GenerateMesh()
    {
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;
        MeshData mesh = new MeshData(width, height);
        int vertIndex = 0;

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                mesh.vertices[vertIndex] = new Vector3(topLeftX + j, noiseMap[i,j], topLeftZ - i);
                mesh.uvs[vertIndex] = new Vector2((float)j/width, (float)i/height);

                if(j < width - 1 && i < height - 1)
                {
                    mesh.addTriangle(vertIndex, vertIndex + 1, vertIndex + width);
                    mesh.addTriangle(vertIndex + width + 1, vertIndex, vertIndex + 1);
                }

                vertIndex++;
            }
        }

        return mesh;
    }

    public void DrawMesh()
    {
        meshFilter.sharedMesh = GenerateMesh().CreateMesh();
        Texture2D texture = new Texture2D(width, height);

        Color[] colors = new Color[width * height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float currentHeight = noiseMap[i, j];
                for (int k = 0; k < regions.Length; k++)
                {
                    if (currentHeight < regions[k].height)
                    {
                        colors[j * width + i] = regions[k].color;
                        break;
                    }

                }
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colors);
        texture.Apply();

        meshRenderer.sharedMaterial.mainTexture = texture;
    }

    private void OnValidate()
    {
        if(width < 1)
            width = 1;
        if(height < 1)
            height = 1;
        if(lacrunarity < 1)
            lacrunarity = 1;
        if(octaves < 0)
            octaves = 0;
    }

}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void addTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
