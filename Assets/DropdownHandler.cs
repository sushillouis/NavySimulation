using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        int pickedEntryIndex = dropdown.value;
        FindObjectOfType<IslandsMgr>().islandSizeMenu = pickedEntryIndex;
    }

    public void GetSizeValue()
    {
        int pickedEntryIndex = dropdown.value;
        FindObjectOfType<IslandsMgr>().islandSizeMenu = pickedEntryIndex;
    }

    public void GetClusterValue()
    {
        int pickedEntryIndex = dropdown.value;
        FindObjectOfType<IslandsMgr>().treeClustersDropdown = pickedEntryIndex;
    }


    // Start is called before the first frame update
    // void Start()
    // {
    //     var dropdown = transform.GetComponent<Dropdown>();

    //     dropdown.options.Clear();
        
    //     List<string> items = new List<string>();
    //     items.Add("Item 1");
    //     items.Add("Item 2");

    //     foreach(var item in items)
    //     {
    //         dropdown.options.Add(new Dropdown.OptionData() {text = item});
    //     }

    //     DropdownItemSelected(dropdown);

    //     dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown);});
    // }

    // void DropdownItemSelected(Dropdown dropdown)
    // {
    //     int index = dropdown.value;
    //     textBox.text = dropdown.options[index].text;
    // }
}
