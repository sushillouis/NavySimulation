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

    [Header("Start Parameters")]
    public CommandCondition startCommandCondition;
    public float startDistanceThreshold;
    public Entity381 startEntity;
    public EntityType startEntityType;
    public bool clearQueueWhenStart;
    public bool insertWhenAdded;

    [Header("Menu")]
    public GameObject commandMenu;
    public GameObject moveMenu;
    public GameObject followMenu;
    public GameObject startMenu;
    public Button moveButton;
    public Button followButton;
    public Button startButton;
    public Color selectedColor;
    public Color passiveColor;

    [Header("Entity Dropdowns")]
    public TMP_Dropdown moveEntityDropdown;
    public TMP_Dropdown followEntityDropdown;
    public TMP_Dropdown startEntityDropdown;

    [Header("VR Menu")]
    public TextMeshProUGUI moveDistanceSliderText;
    public TextMeshProUGUI moveTimeSliderText;
    public TextMeshProUGUI followDistanceSliderText;
    public TextMeshProUGUI followTimeSliderText;
    public TextMeshProUGUI startDistanceSliderText;
    
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

        startCommandCondition = CommandCondition.NoCondition;
        startDistanceThreshold = 1000;
        startEntity = null;
        startEntityType = EntityType.DDG51;
        clearQueueWhenStart = false;
        insertWhenAdded = false;

        initialized = false;
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

    //--------------------------------------------------------------------
    //Move UI
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

    //Not used anynore
    /*
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
    */

    public void SetMoveEntity(int input)
    {
        moveEntity = EntityMgr.inst.entities[input];
    }

    public void SetMoveEntityType(int input)
    {
        moveEntityType = (EntityType)input;
    }


    //--------------------------------------------------------------------
    //Follow UI
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

    //Not used anynore
    /*
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
    */

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

    //--------------------------------------------------------------------
    //Start UI

    public void SetStartCommandCondition(int input)
    {
        startCommandCondition = (CommandCondition)input;
    }

    public void SetStartDistanceThreshold(string input)
    {
        if (input != "")
        {
            float distance = float.Parse(input);
            startDistanceThreshold = distance;
        }
        else
            startDistanceThreshold = 1000;
    }

    public void SetStartDistanceThresholdSlider(float input)
    {
        startDistanceThreshold = input;
        startDistanceSliderText.text = "" + input;
    }

    //Not used anynore
    /*
    public void SetStartEntity(string input)
    {
        startEntity = null;

        foreach (Entity381 ent in EntityMgr.inst.entities)
        {
            if (ent.name.ToLower().Equals(input.ToLower()))
            {
                startEntity = ent;
                break;
            }
        }
    }
    */

    public void SetStartEntity(int input)
    {
        startEntity = EntityMgr.inst.entities[input];
    }

    public void SetStartEntityType(int input)
    {
        startEntityType = (EntityType)input;
    }

    public void SetClearQueueWhenStart(bool input)
    {
        clearQueueWhenStart = input;
    }

    public void SetInsertWhenAdded(bool input)
    {
        insertWhenAdded = input;
    }

    //--------------------------------------------------------------------
    //Menu Control

    public void InitializeEntityDropdown()
    {
        moveEntityDropdown.ClearOptions();
        followEntityDropdown.ClearOptions();
        startEntityDropdown.ClearOptions();
        foreach (Entity381 ent in EntityMgr.inst.entities)
        {
            moveEntityDropdown.options.Add(new TMP_Dropdown.OptionData(ent.name));
            followEntityDropdown.options.Add(new TMP_Dropdown.OptionData(ent.name));
            startEntityDropdown.options.Add(new TMP_Dropdown.OptionData(ent.name));
        }
        moveEntityDropdown.RefreshShownValue();
        followEntityDropdown.RefreshShownValue();
        startEntityDropdown.RefreshShownValue();
}

    public void OpenMoveMenu()
    {
        moveButton.image.color = selectedColor;
        followButton.image.color = passiveColor;
        startButton.image.color = passiveColor;
        moveMenu.SetActive(true);
        followMenu.SetActive(false);
        startMenu.SetActive(false);
    }

    public void OpenFollowMenu()
    {
        moveButton.image.color = passiveColor;
        followButton.image.color = selectedColor;
        startButton.image.color = passiveColor;
        moveMenu.SetActive(false);
        followMenu.SetActive(true);
        startMenu.SetActive(false);
    }

    public void OpenStartMenu()
    {
        moveButton.image.color = passiveColor;
        followButton.image.color = passiveColor;
        startButton.image.color = selectedColor;
        moveMenu.SetActive(false);
        followMenu.SetActive(false);
        startMenu.SetActive(true);
    }
}
