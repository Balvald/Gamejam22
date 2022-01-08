using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConstructor : MonoBehaviour
{
    private TrainStation mSelectedStation;

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
        if (mSelectedStation == null)
        {
            return;
        }

        // TODO: render preview
    }

    public void HandleStationClick(TrainStation station, int lineId = -1)
    {
        if (mSelectedStation == null)
        {
            mSelectedStation = station;
            return;
        }

        if (station == mSelectedStation || 
            station.GetComponent<TrainStation>().HasAdjacent(mSelectedStation) ||
            Vector3.Distance(mSelectedStation.transform.position, station.transform.position) > mMaxDistance)
        {
            
            Debug.Log(
                "buidling Failed: " + 
                (station == mSelectedStation) + ", " + 
                station.GetComponent<TrainStation>().HasAdjacent(mSelectedStation) + ", " + 
                (Vector3.Distance(mSelectedStation.transform.position, station.transform.position) > mMaxDistance));
            mSelectedStation = null;
            return;
        }

        TrainLine line;
        if (mSelectedStation.TryGetTrainLine(lineId) != null)
        {
            line = mSelectedStation.TryGetTrainLine(lineId);
            line.AddStation(station, mSelectedStation);
            mSelectedStation = null;
            return;
        }

        line = Instantiate(mLinePrefab).GetComponent<TrainLine>();
        line.transform.SetParent(transform, false);
        line.Initialize(mSelectedStation, station, mIdCounter);

        // Just for Testing make a Train here
        var train = Instantiate(mTrainPrefab).GetComponent<Train>();
        train.transform.SetParent(line.transform, false);
        train.SetLine(line);
        line.AddTrain(train);

        line.LineColor = mAllowedLineColors[mIdCounter % mAllowedLineColors.Count];

        mIdCounter++;

        mSelectedStation = null;
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
