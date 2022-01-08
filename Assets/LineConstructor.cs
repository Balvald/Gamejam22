using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConstructor : MonoBehaviour
{
    private TrainStation mSelectedStation;

    private TrainStation SelectedStation
    {
        set
        {
            if (value == null)
            {
                if (mSelectedStation != null)
                {
                    mSelectedStation.DeselectStation();
                }
                mSelectedStation = null;
                return;
            }
            mSelectedStation = value;
            mSelectedStation.SelectStation();

        }
        get => mSelectedStation;
    }

    [SerializeField]
    private GameObject mLinePrefab;
    [SerializeField]
    private GameObject mTrainPrefab;

    private int mIdCounter;

    [SerializeField]
    private int mMaxDistance = 200;

    public bool HasSelected => mSelectedStation != null;

    [SerializeField]
    private List<Color> mAllowedLineColors = new List<Color>();

    // Update is called once per frame
    void Update()
    {
        if (SelectedStation == null)
        {
            return;
        }

        // TODO: render preview
    }

    public void HandleStationClick(TrainStation station, int lineId = -1)
    {
        if (SelectedStation == null)
        {
            SelectedStation = station;
            return;
        }

        if (station == SelectedStation || 
            station.GetComponent<TrainStation>().HasAdjacent(SelectedStation) ||
            Vector3.Distance(SelectedStation.transform.position, station.transform.position) > mMaxDistance)
        {
            
            Debug.Log(
                "buidling Failed: " + 
                (station == SelectedStation) + ", " + 
                station.GetComponent<TrainStation>().HasAdjacent(SelectedStation) + ", " + 
                (Vector3.Distance(SelectedStation.transform.position, station.transform.position) > mMaxDistance));
            SelectedStation = null;
            return;
        }

        TrainLine line;
        if (SelectedStation.TryGetTrainLine(lineId) != null)
        {
            line = SelectedStation.TryGetTrainLine(lineId);
            line.AddStation(station, SelectedStation);
            SelectedStation = null;
            return;
        }

        line = Instantiate(mLinePrefab).GetComponent<TrainLine>();
        line.transform.SetParent(transform, false);
        line.Initialize(SelectedStation, station, mIdCounter);

        // Just for Testing make a Train here
        var train = Instantiate(mTrainPrefab).GetComponent<Train>();
        train.transform.SetParent(line.transform, false);
        train.SetLine(line);
        line.AddTrain(train);

        line.LineColor = mAllowedLineColors[mIdCounter % mAllowedLineColors.Count];

        mIdCounter++;

        SelectedStation = null;
    }

    public void ResetIDCounter()
    {
        mIdCounter = 0;
    }

    public int GetCurrentIDCounter()
    {
        return mIdCounter;
    }
}
