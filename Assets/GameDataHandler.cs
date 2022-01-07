using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataHandler : MonoBehaviour
{
    string saveFile;

    public GameObject trainStationManager;
    public GameObject lineConstructor;
    public GameObject resourceManager;
    public GameObject unlockableStationParent;
    [SerializeField]
    private GameObject mTrainPrefab;
    [SerializeField]
    private GameObject mLinePrefab;

    GameData gameData = new GameData();

    void Awake()
    {
        //saveFile = Application.persistentDataPath + @"/saves/slot1.json";
    }

    public void switchSaveSlot(string slot)
    {
        saveFile = Application.persistentDataPath + @"/saves/" + slot;
    }

    public void UpdateGameData()
    {
        UpdateResourceData();
        UpdateStationData();
        UpdateTrainLineData();
    }

    public void UpdateResourceData()
    {
        PlayerResourceData currentPlayerResourceData = new PlayerResourceData();

        ResourceManager resMan = resourceManager.GetComponent<ResourceManager>();
        currentPlayerResourceData.iron = resMan.GetResourceAmount(ResourceType.Iron);
        currentPlayerResourceData.coal = resMan.GetResourceAmount(ResourceType.Coal);
        currentPlayerResourceData.money = resMan.GetResourceAmount(ResourceType.Money);

        gameData.playerResourceData = currentPlayerResourceData;
    }

    public void UpdateStationData()
    {
        List<StationData> currentAllStationData = new List<StationData>();

        int stationIndex = 0;

        object[] potentialStations = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object station in potentialStations)
        {
            GameObject gameObject = (GameObject)station;
            
            // We infact have found a TrainStation
            if (gameObject.GetComponents<TrainStation>().Length != 0)
            {
                TrainStation trainStation = gameObject.GetComponent<TrainStation>();

                StationData currentStationData = new StationData();

                currentStationData.stationName = gameObject.transform.name;

                currentStationData.posX = gameObject.transform.position.x;
                currentStationData.posY = gameObject.transform.position.y;

                currentStationData.ironbuilt = false;
                currentStationData.coalbuilt = false;
                currentStationData.moneybuilt = false;

                currentStationData.unlocked = true;

                foreach (Transform child in gameObject.transform)
                {
                    if (child.GetComponent<ResourceGenerator>() != null)
                    {
                        // child is infact a resourcegenerator.

                        ResourceGenerator resGen = child.GetComponent<ResourceGenerator>();

                        if (resGen.ResourceToGenerate == ResourceType.Iron)
                        {
                            currentStationData.ironbuilt = true;
                        }
                        if (resGen.ResourceToGenerate == ResourceType.Coal)
                        {
                            currentStationData.coalbuilt = true;
                        }
                        if (resGen.ResourceToGenerate == ResourceType.Money)
                        {
                            currentStationData.moneybuilt = true;
                        }
                    }
                }
                
                trainStation.stationDataIndex = stationIndex;
                stationIndex++;
                currentAllStationData.Add(currentStationData);
            }
        }

        foreach (Transform child in unlockableStationParent.transform)
        {
            foreach (Transform grandchild in child)
            {
                StationData currentStationData = new StationData();

                currentStationData.stationName = grandchild.name;

                currentStationData.unlocked = false;
                currentStationData.ironbuilt = false;
                currentStationData.coalbuilt = false;
                currentStationData.moneybuilt = false;

                currentStationData.posX = grandchild.transform.position.x;
                currentStationData.posY = grandchild.transform.position.y;

                TrainStation trainStation = grandchild.GetComponent<TrainStation>();
                trainStation.stationDataIndex = stationIndex;
                stationIndex++;
                currentAllStationData.Add(currentStationData);
            }
        }

        gameData.allStationData = currentAllStationData;

    }

    public void UpdateTrainLineData()
    {
        List<TrainLineData> trainLineDataList = new List<TrainLineData>();

        foreach (Transform child in lineConstructor.transform)
        {
            TrainLine trainLine = child.GetComponent<TrainLine>();
            List<TrainStation> lineStations = trainLine.GetMStations();

            TrainLineData trainLineData = new TrainLineData();
            trainLineData.stationIndices = new int[lineStations.Count];

            int trainLineDataIndex = 0;

            foreach(TrainStation station in lineStations)
            {
                trainLineData.stationIndices[trainLineDataIndex] = station.stationDataIndex;
                trainLineDataIndex++;

            }

            trainLineDataList.Add(trainLineData);

        }

        gameData.allTrainLineData = trainLineDataList;
    }

    public void readFile()
    {
        if (File.Exists(saveFile))
        {
            string fileContents = File.ReadAllText(saveFile);
            gameData = JsonUtility.FromJson<GameData>(fileContents);
        }

        // Destroy All unlocked stations.
        object[] potentialStations = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object station in potentialStations)
        {
            GameObject gameObject = (GameObject)station;

            // We infact have found an unlocked TrainStation
            if (gameObject.GetComponents<TrainStation>().Length != 0)
            {
                GameObject.Destroy(gameObject);
            }
        }

        // Destroy All unlockable stations.
        foreach (Transform child in unlockableStationParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Destroy All trainlines and trains
        foreach (Transform child in lineConstructor.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Rebuild Stations
        RebuildStationsFromData();
        // Rebuild TrainLines and Trains
        lineConstructor.GetComponent<LineConstructor>().ResetIDCounter();
        InstantiateTrainLinesFromData();
        // Give Player his resources specified in Save File
        GiveResourcesBackToPlayer();

    }

    public void GiveResourcesBackToPlayer()
    {
        ResourceManager resMan = resourceManager.GetComponent<ResourceManager>();

        resMan.SetResource(ResourceType.Iron, gameData.playerResourceData.iron);
        resMan.SetResource(ResourceType.Coal, gameData.playerResourceData.coal);
        resMan.SetResource(ResourceType.Money, gameData.playerResourceData.money);
    }

    public void InstantiateTrainLinesFromData()
    {
        // Stations must be existing at this point!

        List<StationData> stationData = gameData.allStationData;

        foreach (TrainLineData trainLineData in gameData.allTrainLineData)
        {
            int id = 0;

            for (int i = 1; i < trainLineData.stationIndices.Length; i++)
            {

                int indexToSearch1 = trainLineData.stationIndices[i-1];
                string nameToSearch1 = stationData[indexToSearch1].stationName;

                Debug.Log(nameToSearch1);

                int indexToSearch2 = trainLineData.stationIndices[i];
                string nameToSearch2 = stationData[indexToSearch2].stationName;

                Debug.Log(nameToSearch2);

                var station1 = GameObject.Find(nameToSearch1);

                var station2 = GameObject.Find(nameToSearch2);

                lineConstructor.GetComponent<LineConstructor>().HandleStationClick(station1.GetComponent<TrainStation>(), id);
                lineConstructor.GetComponent<LineConstructor>().HandleStationClick(station2.GetComponent<TrainStation>(), id);
            }
            id++;
        }
    }

    public void RebuildStationsFromData()
    {
        List<StationData> allStations = gameData.allStationData;

        foreach (StationData station in allStations)
        {
            trainStationManager.GetComponent<TrainStationManager>().CreateStation(station);
        }
    }

    public void writeFile()
    {
        UpdateGameData();
        string jsonString = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFile, jsonString);
    }
}