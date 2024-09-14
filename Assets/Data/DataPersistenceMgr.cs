using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

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

    private void Awake()
    {
        inst = this;
        stateID = 0;
        maxStateID = 0;
    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);

        NewGame();

        string path = Path.Combine(Application.persistentDataPath, "saves");

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllPersistenceObjects();

    }

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
        selectedProfileID = "state" + (stateID + 1);

        SaveGame();

        stateID++;
        maxStateID = stateID;

    }

    public void LoadForward()
    {
        if (stateID == maxStateID)
        {
            Debug.Log("At max state");
            return;
        }

        stateID++;
        selectedProfileID = "state" + stateID;
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
            selectedProfileID = "state" + stateID;
        }

        LoadGame();
    }

    private List<IDataPersistence> FindAllPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
