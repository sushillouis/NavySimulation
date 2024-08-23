using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderMgr : MonoBehaviour
{
    [SerializeField] private Slider treeSlider;
    [SerializeField] private TextMeshProUGUI sliderText;
    public TreeMaker TreeCluster;
    public IslandsMgr islandMgr;

    private void Start()
    {
        sliderText.text = treeSlider.value.ToString("0");

        treeSlider.onValueChanged.AddListener((v) => {
            sliderText.text = v.ToString("0");
        });

        SetSizeValues();
        GetDensityValue();    
    }
    public void GetDensityValue()
    {
        float sliderVal = treeSlider.value;
        TreeCluster.treeCount = sliderVal;
    }

    public void SetSizeValues()
    {
        int sizeValue = islandMgr.islandSizeMenu;
        switch(sizeValue)
        {
            case 0:
                treeSlider.maxValue = 50;
                treeSlider.minValue = 1;
                treeSlider.value = 25;
            break;
            case 1:
                treeSlider.maxValue = 75;
                treeSlider.minValue = 20;
                treeSlider.value = 50;
            break;
            case 2:
                treeSlider.maxValue = 200;
                treeSlider.minValue = 50;
                treeSlider.value = 125;
            break;
        }
        GetDensityValue();
    }
}
