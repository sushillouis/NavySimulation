using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandsMgr : MonoBehaviour
{
    [Header("Move Parameters")]
    public CommandCondition moveCommandCondition;
    public float moveDistanceThreshold;
    public float moveTimeThreshold;
    public Entity381 moveEntity;
    public EntityType moveEntityType;

    [Header("Follow Parameters")]
    public CommandCondition followCommandCondition;
    public float followDistanceThreshold;
    public float followTimeThreshold;
    public Entity381 followEntity;
    public EntityType followEntityType;
    public bool fromFollow;

    [Header("Menu")]
    public GameObject commandMenu;
    public GameObject moveMenu;
    public GameObject followMenu;
    public Button moveButton;
    public Button followButton;
    public Color selectedColor;
    public Color passiveColor;

    [Header("VR Menu")]
    public TextMeshProUGUI moveDistanceSliderText;
    public TextMeshProUGUI moveTimeSliderText;
    public TMP_Dropdown moveEntityDropdown;
    public TextMeshProUGUI followDistanceSliderText;
    public TextMeshProUGUI followTimeSliderText;
    public TMP_Dropdown followEntityDropdown;
    bool initialized;

    public static CommandsMgr inst;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;

        moveCommandCondition = CommandCondition.NoCondition;
        moveDistanceThreshold = 1000;
        moveTimeThreshold = 60;
        moveEntity = null;
        moveEntityType = EntityType.DDG51;

        followCommandCondition = CommandCondition.NoCondition;
        followDistanceThreshold = 1000;
        followTimeThreshold = 60;
        followEntity = null;
        followEntityType = EntityType.DDG51;
        fromFollow = false;

        if (moveEntityDropdown != null || followEntityDropdown != null)
            initialized = false;
        else
            initialized = true;
}

    // Update is called once per frame
    void Update()
    {
        if(!initialized)
        {
            initialized = true;
            InitializeEntityDropdown();
        }

        if (Input.GetKeyDown(KeyCode.F1) || (VRControlMgr.inst != null && VRControlMgr.inst.YButtonPress.action.WasPressedThisFrame()))
        {
            commandMenu.SetActive(!commandMenu.activeSelf);
        }
    }

    public void SetMoveCommandCondition(int input)
    {
        moveCommandCondition = (CommandCondition)input;
    }

    public void SetMoveDistanceThreshold(string input)
    {
        if (input != "")
        {
            float distance = float.Parse(input);
            moveDistanceThreshold = distance;
        }
        else
            moveDistanceThreshold = 1000;
    }

    public void SetMoveDistanceThresholdSlider(float input)
    {
        moveDistanceThreshold = input;
        moveDistanceSliderText.text = "" + input;
    }

    public void SetMoveTimeThreshold(string input)
    {
        if (input != "")
        {
            float distance = float.Parse(input);
            moveTimeThreshold = distance;
        }
        else
            moveTimeThreshold = 60;
    }

    public void SetMoveTimeThresholdSlider(float input)
    {
        moveTimeThreshold = input;
        moveTimeSliderText.text = "" + input;
    }

    public void SetMoveEntity(string input)
    {
        moveEntity = null;

        foreach (Entity381 ent in EntityMgr.inst.entities)
        {
            if (ent.name.ToLower().Equals(input.ToLower()))
            {
                moveEntity = ent;
                break;
            }
        }
    }

    public void SetMoveEntity(int input)
    {
        moveEntity = EntityMgr.inst.entities[input];
    }

    public void SetMoveEntityType(int input)
    {
        moveEntityType = (EntityType)input;
    }

    public void SetFollowCommandCondition(int input)
    {
        followCommandCondition = (CommandCondition)input;
    }

    public void SetFollowDistanceThreshold(string input)
    {
        if (input != "")
        {
            float distance = float.Parse(input);
            followDistanceThreshold = distance;
        }
        else
            followDistanceThreshold = 1000;
    }

    public void SetFollowDistanceThresholdSlider(float input)
    {
        followDistanceThreshold = input;
        followDistanceSliderText.text = "" + input;
    }

    public void SetFollowTimeThreshold(string input)
    {
        if (input != "")
        {
            float distance = float.Parse(input);
            followTimeThreshold = distance;
        }
        else
            followTimeThreshold = 60;
    }

    public void SetFollowTimeThresholdSlider(float input)
    {
        followTimeThreshold = input;
        followTimeSliderText.text = "" + input;
    }

    public void SetFollowEntity(string input)
    {
        followEntity = null;

        foreach (Entity381 ent in EntityMgr.inst.entities)
        {
            if (ent.name.ToLower().Equals(input.ToLower()))
            {
                followEntity = ent;
                break;
            }
        }
    }

    public void SetFollowEntity(int input)
    {
        followEntity = EntityMgr.inst.entities[input];
    }

    public void SetFollowEntityType(int input)
    {
        followEntityType = (EntityType)input;
    }

    public void SetFromFollow(int input)
    {
        if (input == 0)
            fromFollow = false;
        else if (input == 1)
            fromFollow = true;
    }

    public void InitializeEntityDropdown()
    {
        if(moveEntityDropdown != null)
        {
            moveEntityDropdown.ClearOptions();
            foreach (Entity381 ent in EntityMgr.inst.entities)
            {
                moveEntityDropdown.options.Add(new TMP_Dropdown.OptionData(ent.name));
            }
            moveEntityDropdown.RefreshShownValue();
        }

        if (followEntityDropdown != null)
        {
            followEntityDropdown.ClearOptions();
            foreach (Entity381 ent in EntityMgr.inst.entities)
            {
                followEntityDropdown.options.Add(new TMP_Dropdown.OptionData(ent.name));
            }
            followEntityDropdown.RefreshShownValue();
        }
    }

    public void OpenMoveMenu()
    {
        moveButton.image.color = selectedColor;
        followButton.image.color = passiveColor;
        moveMenu.SetActive(true);
        followMenu.SetActive(false);
    }

    public void OpenFollowMenu()
    {
        moveButton.image.color = passiveColor;
        followButton.image.color = selectedColor;
        moveMenu.SetActive(false);
        followMenu.SetActive(true);
    }
}
