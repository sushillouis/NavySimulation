using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.UI;

public class DataPersistenceMgr : MonoBehaviour
{
    public static DataPersistenceMgr inst;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    FileDataHandler dataHandler;

    public int stateID;
    public int maxStateID;
    public string selectedProfileID = "test";

    public bool record;

    public Slider rewindSlider;

    private void Awake()
    {
        inst = this;
        stateID = 0;
        maxStateID = 0;

        rewindSlider.maxValue = maxStateID;
        rewindSlider.value = stateID;
        //record = true;
    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);

        NewGame();

        string path = Path.Combine(Application.persistentDataPath, "saves");

        this.dataHandler = new FileDataHandler(path, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllPersistenceObjects();

    }

    float frame = 0;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.RightBracket)) 
        {
            LoadForward();
        }

        if(Input.GetKeyUp(KeyCode.LeftBracket))
        {
            LoadBackward();
        }
    }

    private void LateUpdate()
    {
        frame++;
        if (frame > 60 && record)
        {
            HandleSaveState();
            frame = 0;
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load(selectedProfileID);
        
        if(this.gameData == null)
        {
            Debug.Log("No data found when loading game");
            NewGame();
        }

        foreach(IDataPersistence p in dataPersistenceObjects)
        {
            p.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        //gameData.Clear();

        foreach (IDataPersistence p in dataPersistenceObjects)
        {
            p.SaveData(gameData);
        }

        dataHandler.Save(gameData, selectedProfileID);
    }

    public void HandleSaveState()
    {
        selectedProfileID = "state" + (stateID + 1) + ".game";

        SaveGame();

        stateID++;
        maxStateID = stateID;

        rewindSlider.maxValue = maxStateID;
        rewindSlider.value = stateID;

    }

    public void LoadForward()
    {
        if (stateID == maxStateID)
        {
            Debug.Log("At max state");
            return;
        }

        stateID++;
        selectedProfileID = "state" + stateID + ".game";
        LoadGame();
        
    }

    public void LoadBackward()
    {
        if (stateID <= 1)
        {
            Debug.Log("At min state");
        }
        else
        {
            stateID--;
            selectedProfileID = "state" + stateID + ".game";
        }

        LoadGame();
    }

    public void LoadState(float stateID)
    {
        selectedProfileID = "state" + (int) stateID + ".game";
        
        if(stateID != this.stateID)
        {
            LoadGame();
            this.stateID = (int)stateID;
        }
            
    }

    private List<IDataPersistence> FindAllPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
