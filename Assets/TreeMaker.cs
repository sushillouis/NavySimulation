using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Analytics;
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

    [System.Serializable]
    public class Trees
    {
        public float treeCount;
        public int clusterNum;
    }
    public int islandChoice;
    public IslandsMgr islandMgr;
    public GameObject[] treePrefab;
    public GameObject treeTray;

    public List<Trees> islandTrees;
    public List<Trees> islandTrees2;
    public Transform clusterParent;

    [HideInInspector]
    public List<GameObject> treeList = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> duplicateClusterList;
    public int leafTexture = 0;
    public float scale;

    public int totalClusters = 0;
    
    private void Start()
    {
        islandTrees.Clear();
        // Trees newTree = new Trees();
        for(int i = 0; i <= 2; i++){
            Trees newTree = new Trees();
            newTree.treeCount = 25;
            newTree.clusterNum = 0;
            islandTrees.Add(newTree);}
    }


    //Create the given number of clusters
    public void CreateClusters()
    {
        for(int i = 0; i < islandMgr.islandCount; i++)
            islandTrees2.Add(islandTrees[i]);

        int j = 0;
        foreach(Trees t in islandTrees2)
        {
            if(t.clusterNum != 0)
            {
                // MakeTrees(treeCount);
                for(int i = 0; i < t.clusterNum; i++)
                {
                    IslandSize islandScale = islandMgr.islandParameters[j].size;
                    MakeTrees(t.treeCount, islandScale);
                    GameObject cluster = Instantiate(treeTray, new Vector3(25500, 0, 25500), Quaternion.identity, clusterParent);
                    duplicateClusterList.Add(cluster);
                    totalClusters++;
                    DeleteMainTrees();
                }
                j++;
            }
        }
    }

    //Create the location for the trees spawned on the island
    public void MakeTrees(float treeCount, IslandSize islandScale)
    {
        FindObjectOfType<TreeColor>().UpdateLeafTexture(leafTexture);
        treeTray.transform.position = new Vector3(25500, 0, 25500);
        switch(islandScale)
        {
            case IslandSize.Large:
                scale = 0;
            break;
            case IslandSize.Medium:
                scale = 250;
            break;
            case IslandSize.Small:
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
        islandTrees2.Clear();
        totalClusters = 0;
    }

    public void DeleteMainTrees()
    {
        for (var i = treeTray.transform.childCount - 1; i >= 1; i--)
        {
            if (Application.isPlaying)
                Destroy(treeTray.transform.GetChild(i).gameObject);
            else
            {
                DestroyImmediate(treeTray.transform.GetChild(i).gameObject); 
            }
        }

    }
}
