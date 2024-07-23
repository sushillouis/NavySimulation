using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderMgr : MonoBehaviour
{
    [SerializeField] private Slider treeSlider;
    [SerializeField] private TextMeshProUGUI sliderText;

    private void Start()
    {
        sliderText.text = treeSlider.value.ToString("0");

        treeSlider.onValueChanged.AddListener((v) => {
            sliderText.text = v.ToString("0");
        });
    }
    public void GetDensityValue()
    {
        float sliderVal = treeSlider.value;
        FindObjectOfType<IslandsMgr>().treeDensitySlider = sliderVal;
    }
}
