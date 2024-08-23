using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    public TreeMaker TreeCluster;
    public Material baseMat;

    private void Start()
    {
        GetSizeValue();
        GetClusterValue();
        GetTextureValue();
    }

    public void GetSizeValue()
    {
        int pickedEntryIndex = dropdown.value;
        FindObjectOfType<IslandsMgr>().islandSizeMenu = pickedEntryIndex;
        switch(pickedEntryIndex)
        {
            case 0:
                baseMat.SetFloat("_HeightCutoff", 86);
            break;
            case 1:
                baseMat.SetFloat("_HeightCutoff", 100);
            break;
            case 2:
                baseMat.SetFloat("_HeightCutoff", 120);
            break;
        }
    }

    public void GetClusterValue()
    {
        int pickedEntryIndex = dropdown.value;
        TreeCluster.clusterNum = pickedEntryIndex;
    }

    public void GetTextureValue()
    {
        int pickedEntryIndex = dropdown.value;
        FindObjectOfType<IslandsMgr>().textureChoice = pickedEntryIndex;
        FindObjectOfType<TreeMaker>().leafTexture = pickedEntryIndex;
    }
}
