using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class TreeMgr : MonoBehaviour
{
    [ContextMenu("ClusterMove")]
    public void ClusterMove()
    {
        multipleIslands();
    }

    public GameObject TreeTray;
    // public GameObject Islands;
    public IslandsMgr islandMgr;
    public RaycastHandler rayHandler;
    public int spawnHeight = 180;

    public float scale = 0;

    // public GameObject[] islandClusters;

    public void multipleIslands()
    {
        List<GameObject> islands = islandMgr.islands;
        GameObject[] clusterCounter = GameObject.FindGameObjectsWithTag("TreeCluster"); 
        List<GameObject> islandClusters = new List<GameObject>();
        int currentCluster = 1;
        int islandScale;
        int j = 0;
        int totalClusters = TreeTray.GetComponent<TreeMaker>().islandTrees2[j].clusterNum;
        foreach(GameObject i in islands)
        {
            // totalClusters = TreeTray.GetComponent<TreeMaker>().islandTrees2[j].clusterNum;
            for(; currentCluster <= totalClusters; currentCluster++)
            {
                islandClusters.Add(clusterCounter[currentCluster]); 
            }
            islandScale = islandMgr.islandParameters[j].islandSizeMenu;
            clusterMover(i, islandClusters, islandScale);
            islandClusters.Clear();
            if(islandMgr.formation != IslandFormation.Single)
            {
                if(j < 2)
                    totalClusters += TreeTray.GetComponent<TreeMaker>().islandTrees2[j + 1].clusterNum;
            }
            j++;
        }

    }

    //Find the random point on which to move the prefab to
    public void clusterMover(GameObject island, List<GameObject> clusterCount, int islandScale)
    {
        // int islandScale = islandMgr.islandSizeMenu;
        switch(islandScale)
        {
            case 2:
                scale = 5000;
            break;
            case 1:
                scale = 2500;
            break;
            case 0:
                scale = 1000;
            break;
        }
        spawnHeight = spawnHeightFinder(scale);
        float maxPtX = island.transform.position.x + scale; 
        float maxPtZ = island.transform.position.z + scale; 
        float minPtX = island.transform.position.x - scale;
        float minPtZ = island.transform.position.z - scale;
        float noiseRef = 0;
        
        foreach(GameObject treeTray in clusterCount)
        {
            Vector3 randomSpawnPos;
            do{
                randomSpawnPos = new Vector3(Random.Range(minPtX, maxPtX), 1000, Random.Range(minPtZ, maxPtZ));
            } while(rayHandler.FindLand(randomSpawnPos, ref noiseRef, spawnHeight) == false);
            treeTray.transform.position = randomSpawnPos;
        }

        foreach(GameObject treeTray in clusterCount)
            treeAdjuster(treeTray, spawnHeight);
    }


    //Adjust each tree to the island height and make sure there's no trees where the water should be
    public void treeAdjuster(GameObject cluster, int spawn)
    {
        List<GameObject> trayList = cluster.GetComponent<TreeMaker>().treeList;
        foreach (GameObject placedTree in trayList)
        {
            float yValue = 0;
            if (placedTree != null){
                bool landFound = rayHandler.FindLand(new Vector3(placedTree.transform.position.x, 1000, placedTree.transform.position.z), ref yValue, spawn);
                
                if (landFound == true)
                    placedTree.transform.position = new Vector3(placedTree.transform.position.x, yValue, placedTree.transform.position.z);

                else
                {
                    if (Application.isPlaying)
                        Destroy(placedTree);
                    else
                        DestroyImmediate(placedTree); 
                }
            }
                
        }

    }

    //Determine the height at which trees should spawn on the island
    public int spawnHeightFinder(float scale)
    {
        int spawnHeight = 0;

        switch (scale)
        {
            case 5000:
                spawnHeight = 150;
            break;
            case 2500:
                spawnHeight = 40;
            break;
            case 1000:
                spawnHeight = 10;
            break;
        }

        return spawnHeight;
    }
}
