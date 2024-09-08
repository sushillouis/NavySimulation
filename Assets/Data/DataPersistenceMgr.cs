using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceMgr : MonoBehaviour
{
    public static DataPersistenceMgr inst;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    FileDataHandler dataHandler;

    private int stateID;
    private string selectedProfileID = "test";

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);

        NewGame();

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllPersistenceObjects();

    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.F2)) 
        { 
            SaveGame();
        }

        if(Input.GetKeyUp(KeyCode.F3))
        {
            LoadGame();
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

    private List<IDataPersistence> FindAllPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
