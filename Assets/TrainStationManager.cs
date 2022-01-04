using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainStationManager : MonoBehaviour
{
    public GameObject trainStationPrefab;
    public GameObject CSVHandler;
    public string datapath = "./Assets/test.csv";
    public List<string[]> StationData;
    private CultureInfo cul = CultureInfo.InvariantCulture;

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
        ReadCSV script = CSVHandler.GetComponent<ReadCSV>();
        StationData = script.ReadCSVFile(datapath);
    }

    void Start()
    {
        CreateStations(StationData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public void UpdateStationData(Transform Container.transform)
    //{
    //    foreach (Transform item in Container.transform)
    //    {
    //        //Do stuff
    //    }
    //}


    public void CreateStations(List<string[]> trainStationData)
    {
        foreach (string[] data in trainStationData)
        {
            Vector3 currentTrainStationPosition = new Vector3(float.Parse(data[1], cul) * 0.01f, float.Parse(data[2], cul) * 0.01f, 0) + new Vector3(-3951, -52714, -4);
            var currentTrainStation = Instantiate(trainStationPrefab, currentTrainStationPosition, Quaternion.identity);
            currentTrainStation.name = data[0];
            currentTrainStation.transform.SetParent(transform, false);
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

}
