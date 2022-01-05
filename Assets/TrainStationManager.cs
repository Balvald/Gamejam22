using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainStationManager : MonoBehaviour
{
    public GameObject trainStationPrefab;
    public GameObject CSVHandler;
    public string datapath;
    public List<string[]> StationData;

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
        GetGameStationData();
    }

    void Start()
    {
        CreateStations(StationData, false);
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
        Debug.Log("Creating Stations");
        Vector3 offset = new Vector3(0, 0, 0);
        float factor = 1.0f;
        if (geo)
        {
            offset = new Vector3(-3951, -52714, -4);
            factor = 0.01f;
        }
        foreach (string[] data in trainStationData)
        {
            Debug.Log("Creating: " + data[0] + " at: " + data[1] + " ; " + data[2]);
            Vector3 currentTrainStationPosition = new Vector3(float.Parse(data[1], cul) * factor, float.Parse(data[2], cul) * factor, 0) + offset;
            var currentTrainStation = Instantiate(trainStationPrefab, currentTrainStationPosition, Quaternion.identity);
            currentTrainStation.name = data[0];
            currentTrainStation.transform.SetParent(transform, false);
            currentTrainStation.AddComponent<ToolTipAccessor>().UpdateToolTipString(currentTrainStation.name);
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

        foreach (Transform station in transform)
        {
            StationData.Add(new string[] {station.name, station.position.x.ToString(nfi), station.position.y.ToString(nfi) });
        }
    }

    public void SaveStationData()
    {
        UpdateStationData();
        WriteCSV script = CSVHandler.GetComponent<WriteCSV>();
        script.WriteCSVofCurrentlyDisplayedStations(StationData);
    }
}
