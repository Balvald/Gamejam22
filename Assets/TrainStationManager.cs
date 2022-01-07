using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class TrainStationManager : MonoBehaviour
{
    public GameObject trainStationPrefab;
    public GameObject PayWallPrefab;
    public GameObject CSVHandler;
    public GameObject UnlockableStationParent;
    public string datapath;
    public List<string[]> StationData;
    private TrainStation[] trainStations;

    // Culture Crap: Needed to force that we use decimal points when converting numbers to strings and back.
    private CultureInfo cul = CultureInfo.InvariantCulture; // using this in Parse forces it to read a "." as a decimal point if the string parsed is indeed a number
    NumberFormatInfo nfi = new NumberFormatInfo(); // We need a number format to force toString to write decimal points "." instead of decimal commas ",".

   // Idea creating all the stations we have at the start.
   // Player selects the first station as his Main hub from where his trian empire expands.

   // TrainStation Manager is going to instatiate all the Trainstations at the start so that they exist.
   // TrainStation Manager is going to set trainstations active or inactive during runtime.

   // Example when train station should be set inactive from an active state:

   // Once First Train station is selected the others are set as inactive since they should be build.


   // Example where trainstation should be set as active from being inactive:

   // When unlocking the station (?) (by having built a station closeby) and enough resources(not implemented yet) are present at the station that is closeby we set it to active.
   //Do we unlock stations or do we let players create stations themselves?

    // Start is called before the first frame update

    void Awake()
    {
        // Set Number Format Info accordingly.
        nfi.NumberDecimalSeparator = ".";
        //GetGeoStationData();
        //GetGameStationData();
        //CreateStations(StationData, false);
    }

    void Start()
    {
        //CreateStations(StationData, false);
    }

    // Update is called once per frame
    void Update()
    {
        // pass
    }

    void GetGeoStationData()
    {
        ReadCSV script = CSVHandler.GetComponent<ReadCSV>();
        StationData = script.ReadCSVFile(datapath);
    }

    void GetGameStationData()
    {
        ReadCSV script = CSVHandler.GetComponent<ReadCSV>();
        StationData = script.ReadCustomCSVFile(datapath);
    }

    public void CreateStations(List<string[]> trainStationData, bool geo)
    {
        //Debug.Log("Creating Stations");
        Vector3 offset = new Vector3(0, 0, 0);
        float factor = 1.0f;
        if (geo)
        {
            offset = new Vector3(-3951, -52714, -4);
            factor = 0.01f;
        }
        foreach (string[] data in trainStationData)
        {
            //Debug.Log("Creating: " + data[0] + " at: " + data[1] + " ; " + data[2]);
            Vector3 currentTrainStationPosition = new Vector3(float.Parse(data[1], cul) * factor, float.Parse(data[2], cul) * factor, 0) + offset;
            GameObject currentTrainStation = Instantiate(trainStationPrefab, currentTrainStationPosition, Quaternion.identity);
            currentTrainStation.name = data[0];
            currentTrainStation.transform.localScale = new Vector3(5, 5, 1);
            //currentTrainStation.transform.SetParent(transform, false);
            currentTrainStation.AddComponent<ToolTipAccessor>().UpdateToolTipString(currentTrainStation.name);

            if (data[3] == "0") // Station is not unlocked yet: Need to create Paywall
            {
                var newUnlockableStationPaywall = Instantiate(PayWallPrefab, currentTrainStationPosition, Quaternion.identity);
                newUnlockableStationPaywall.transform.SetParent(UnlockableStationParent.transform, true);
                newUnlockableStationPaywall.transform.localScale = new Vector3(1, 1, 1);
                newUnlockableStationPaywall.GetComponent<PayWall>().SetObjectToUnlock(currentTrainStation);
                newUnlockableStationPaywall.AddComponent<ToolTipAccessor>().UpdateToolTipString(currentTrainStation.name + " (Not Owned)");
                currentTrainStation.transform.SetParent(newUnlockableStationPaywall.transform, true);
            }
        }
    }

    public void CreateStation(StationData stationData)
    {
        //Debug.Log("Creating: " + data[0] + " at: " + data[1] + " ; " + data[2]);
        Vector3 currentTrainStationPosition = new Vector3(stationData.posX, stationData.posY, 0);
        GameObject currentTrainStation = Instantiate(trainStationPrefab, currentTrainStationPosition, Quaternion.identity);
        currentTrainStation.name = stationData.stationName;
        currentTrainStation.transform.localScale = new Vector3(5, 5, 1);
        //currentTrainStation.transform.SetParent(transform, false);
        currentTrainStation.AddComponent<ToolTipAccessor>().UpdateToolTipString(currentTrainStation.name);

        if (!stationData.unlocked) // Station is not unlocked yet: Need to create Paywall
        {
            var newUnlockableStationPaywall = Instantiate(PayWallPrefab, currentTrainStationPosition, Quaternion.identity);
            newUnlockableStationPaywall.transform.SetParent(UnlockableStationParent.transform, true);
            newUnlockableStationPaywall.transform.localScale = new Vector3(1, 1, 1);
            newUnlockableStationPaywall.GetComponent<PayWall>().SetObjectToUnlock(currentTrainStation);
            newUnlockableStationPaywall.AddComponent<ToolTipAccessor>().UpdateToolTipString(currentTrainStation.name + " (Not Owned)");
            currentTrainStation.transform.SetParent(newUnlockableStationPaywall.transform, true);
        }

        TrainStation trainStation = currentTrainStation.GetComponent<TrainStation>();

        // TODO: Write the saved efficiency values into the file.

        // Put efficiency values into station where it is appropiate.

        // Need to create Resource Generators

        if (stationData.ironbuilt)
        {
            trainStation.AddProducer(ResourceType.Iron);
        }

        if (stationData.coalbuilt)
        {
            trainStation.AddProducer(ResourceType.Coal);
        }

        if (stationData.moneybuilt)
        {
            trainStation.AddProducer(ResourceType.Money);
        }

    }

    public void SetStationInactive(GameObject trainStation)
    {
        trainStation.SetActive(true);
    }

    public void SetStationActive(GameObject trainStation)
    {
        trainStation.SetActive(false);
    }

    public void UpdateStationData()
    {
        StationData = new List<string[]>();

        object[] potentialStations = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object station in potentialStations)
        {
            GameObject gameObject = (GameObject)station;
            //Debug.Log(gameObject.name);

            // We infact have found a TrainStation
            if (gameObject.GetComponents<TrainStation>().Length != 0)
            {
                string unlockedvalue = "1";

                StationData.Add(new string[] {gameObject.name, gameObject.transform.position.x.ToString(nfi), gameObject.transform.position.y.ToString(nfi), unlockedvalue});
            }
        }

        foreach (Transform child in UnlockableStationParent.transform)
        {
            foreach (Transform grandchild in child)
            {
                string unlockedvalue = "0";

                StationData.Add(new string[] {grandchild.name, grandchild.transform.position.x.ToString(nfi), grandchild.transform.position.y.ToString(nfi), unlockedvalue});
            }
        }
    }
    /*
    public void SaveStationData()
    {
        UpdateStationData();
        WriteCSV script = CSVHandler.GetComponent<WriteCSV>();
        script.WriteCSVofCurrentlyDisplayedStations(StationData);
    }
    */
}
