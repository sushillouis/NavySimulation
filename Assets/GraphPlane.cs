using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//struct to hold individual ship data
public struct ShipData
{
    public float mass;
    public Vector3 position;
    public Vector3 movePosition;
    public int commands;
};

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GraphPlane : MonoBehaviour
{
    Mesh mesh;
    MeshFilter meshFilter;

    public List<Vector3> vertices;
    List<int> triangles;
    List<ShipData> allShipData; //list for individual ship data
    public ComputeShader potentialShader; //compute shader used for calculation
    public Vector2 size;
    public int resolution;
    public Entity381 entity; //entity that the graph is calculating for

    //initializes the graph mesh
    void Awake()
    {
        //Create and set mesh
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        //Change mesh format, this needs to be done for graphs with large resolutions
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        //Set mesh size and resolutions
        size = GraphMgr.inst.size;
        resolution = GraphMgr.inst.resolution;
        resolution = Mathf.Clamp(resolution, 0, 1000);

        //Creates and sets triangles and vertices of the mesh
        GeneratePlane(size, resolution);
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        allShipData = new List<ShipData>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeights(entity);
        UpdateMesh();
    }

    //calculates the mesh triangles and vertices
    void GeneratePlane(Vector2 size, int resolution)
    {           
        //adds vertices of mesh evenly spaced vertices out
        vertices = new List<Vector3>();

        float xPerStep = size.x / resolution;
        float yPerStep = size.y / resolution;
        for (int y = 0; y < resolution + 1; y++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                vertices.Add(new Vector3(x * xPerStep, 0, y * yPerStep));
            }
        }

        //creates triangles of mesh
        triangles = new List<int>();

        for (int row = 0; row < resolution; row++)
        {
            for (int col = 0; col < resolution; col++)
            {
                int i = (row * resolution) + +row + col;

                triangles.Add(i);
                triangles.Add(i + resolution + 1);
                triangles.Add(i + resolution + 2);

                triangles.Add(i);
                triangles.Add(i + resolution + 2);
                triangles.Add(i + 1);
            }
        }
    }

    //updates the mesh vertices, bounds, normals, and tangents
    void UpdateMesh()
    {
        mesh.vertices = vertices.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    //calculates heights of mesh vertices based on world position of the vertices
    void UpdateHeights(Entity381 currentEnt)
    {
        //sends ship specific information to the compute shader via compute buffer
        updateShipData();
        ComputeBuffer shipsBuffer = new ComputeBuffer(allShipData.Count, sizeof(float) * 7 + sizeof(int));
        shipsBuffer.SetData(allShipData);

        //sends world position of mesh vertices to the compute shader via compute buffer
        ComputeBuffer vertexBuffer = new ComputeBuffer(vertices.Count, sizeof(float) * 3);
        Vector3[] worldVert = vertices.ToArray();
        transform.TransformPoints(worldVert); //transforms local mesh coordinates to world cordinates
        vertexBuffer.SetData(worldVert);

        //sets variables in the compute shader
        potentialShader.SetInt("numShips", EntityMgr.inst.entities.Count);
        potentialShader.SetInt("entity", EntityMgr.inst.entities.IndexOf(currentEnt));
        potentialShader.SetFloat("maxMag", GraphMgr.inst.maxMag);
        potentialShader.SetFloat("attractiveExponent", AIMgr.inst.attractiveExponent);
        potentialShader.SetFloat("attractiveCoefficient", AIMgr.inst.attractionCoefficient);
        potentialShader.SetFloat("repulsiveExponent", AIMgr.inst.repulsiveExponent);
        potentialShader.SetFloat("repulsiveCoefficient", AIMgr.inst.repulsiveCoefficient);
        potentialShader.SetFloat("threshold", AIMgr.inst.potentialDistanceThreshold);
        potentialShader.SetBuffer(0, "ships", shipsBuffer);
        potentialShader.SetBuffer(0, "positions", vertexBuffer);

        //starts calculations in buffer
        potentialShader.Dispatch(0, (vertices.Count / 64) + 1, 1, 1);

        //retrieves calculations and sets new vertices
        vertexBuffer.GetData(worldVert);
        transform.InverseTransformPoints(worldVert); //transforms world coordinates back to local coordinates
        vertices = new List<Vector3>(worldVert);

        //disposes buffers after use
        shipsBuffer.Dispose();
        vertexBuffer.Dispose();
    }

    //gets data from each ship to be used in potential calculations
    public void updateShipData()
    {
        allShipData = new List<ShipData>();
        foreach(Entity381 entity in EntityMgr.inst.entities) 
        { 
            ShipData shipData = new ShipData();
            shipData.mass = entity.mass;
            shipData.position = entity.position;
            shipData.commands = entity.transform.GetComponent<UnitAI>().commands.Count;
            if (entity.transform.GetComponent<UnitAI>().commands.Count != 0)
                shipData.movePosition = entity.transform.GetComponent<UnitAI>().moves[0].movePosition;
            else
                shipData.movePosition = Vector3.zero;
            allShipData.Add(shipData);

        }
    }
}