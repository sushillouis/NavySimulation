using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeMaker : MonoBehaviour
{
    [ContextMenu("TestTree")]
    public void TestTree()
    {
        DeleteAllTrees();
        CreateClusters();
    }

    [ContextMenu("DeleteTree")]
    public void DeleteTree()
    {
        DeleteAllTrees();
    }

    public float treeCount;
    public int clusterNum;
    public GameObject[] treePrefab;
    public GameObject treeTray;

    [HideInInspector]
    public List<GameObject> treeList = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> duplicateClusterList;
    public Transform clusterParent;

    public int islandSizeTest = 0;
    public int leafTexture = 0;
    public float scale;
    
    //Create the given number of clusters
    public void CreateClusters()
    {
        if(clusterNum != 0)
        {
            MakeTrees(treeCount);

            if(clusterNum > 1)
            {
                for(int i = 0; i < (clusterNum * FindObjectOfType<IslandsMgr>().islandCount) - 1; i++)
                {
                    GameObject cluster = Instantiate(treeTray, new Vector3(25500, 0, 25500), Quaternion.identity, clusterParent);
                    duplicateClusterList.Add(cluster);
                }
            }
        }
    }

    //Create the location for the trees spawned on the island
    public void MakeTrees(float treeCount)
    {
        FindObjectOfType<TreeColor>().UpdateLeafTexture(leafTexture);
        treeTray.transform.position = new Vector3(25500, 0, 25500);
        int islandScale = FindObjectOfType<IslandsMgr>().islandSizeMenu;
        switch(islandScale)
        {
            case 2:
                scale = 0;
            break;
            case 1:
                scale = 250;
            break;
            case 0:
                scale = 350;
            break;
        }

        for(int i = 0; i <= treeCount - 1; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(25000 + scale, 26000 - scale), 0, Random.Range(25000 + scale, 26000 - scale));
            treeList.Add(GenerateTree(randomSpawnPosition));
        }
    }

    //Instatiate the actual trees
    public GameObject GenerateTree(Vector3 spawnPos)
    {
        float yTilt = Random.Range(1, 360);
        Quaternion randomTilt = Quaternion.Euler(0, yTilt, 0);
        GameObject objectToSpawn = treePrefab[Random.Range(0, treePrefab.Length)];
        GameObject treeObject = Instantiate(objectToSpawn, spawnPos, randomTilt);
        treeObject.transform.SetParent(transform);

        return treeObject;
    }

    //Delete the trees and extra clusters
    public void DeleteAllTrees()
    {
        foreach(GameObject cluster in duplicateClusterList)
        {
            if (Application.isPlaying)
            {
                if (cluster != null)
                Destroy(cluster);
            }
            else
            {
                DestroyImmediate(cluster); 
            }
        }
        duplicateClusterList.Clear();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("TreeDestroy");
        foreach(GameObject go in gos)
        {
            if (Application.isPlaying)
                Destroy(go);
            else
            {
                DestroyImmediate(go); 
            }
        } 
        treeList.Clear();
    }
}
