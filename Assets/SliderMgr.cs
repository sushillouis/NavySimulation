using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderMgr : MonoBehaviour
{
    public Slider treeSlider;
    [SerializeField] private TextMeshProUGUI sliderText;
    public TreeMaker TreeCluster;
    public IslandsMgr islandMgr;

    private void Start()
    {
        sliderText.text = treeSlider.value.ToString("0");

        treeSlider.onValueChanged.AddListener((v) => {
            sliderText.text = v.ToString("0");
        });

        // for(int i = 0; i <= islandMgr.islandCount - 1; i++)
        // {
        //     TreeCluster.islandTrees[i].treeCount = 25;
        //     TreeCluster.islandTrees[i].clusterNum = 0;
        // }

        findSlider();
        // GetDensityValue();    
    }
    public void GetDensityValue()
    {
        float sliderVal = treeSlider.value;
        TreeCluster.islandTrees[TreeCluster.islandChoice].treeCount = sliderVal;
    }

    public void findSlider()
    {
        int treesFound = TreeCluster.islandTrees[TreeCluster.islandChoice].clusterNum;
        treeSlider.value = treesFound;
    }

    public void SetSizeValues()
    {
        IslandSize sizeValue = FindObjectOfType<IslandsMgr>().islandParameters[TreeCluster.islandChoice].size;
        switch(sizeValue)
        {
            case IslandSize.Small:
                treeSlider.maxValue = 50;
                treeSlider.minValue = 1;
                treeSlider.value = 25;
            break;
            case IslandSize.Medium:
                treeSlider.maxValue = 75;
                treeSlider.minValue = 20;
                treeSlider.value = 50;
            break;
            case IslandSize.Large:
                treeSlider.maxValue = 200;
                treeSlider.minValue = 50;
                treeSlider.value = 125;
            break;
        }
    }
}
