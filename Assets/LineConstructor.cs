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

    // Update is called once per frame
    void Update()
    {
        if (mSelectedStation == null)
        {
            return;
        }

        // TODO: render preview
    }

    public void HandleStationClick(TrainStation station)
    {
        if (mSelectedStation == null)
        {
            mSelectedStation = station;
            return;
        }

        if (station == mSelectedStation || station.GetComponent<TrainStation>().HasAdjacent(mSelectedStation))
        {
            mSelectedStation = null;
            return;
        }

        TrainLine line;
        if (mSelectedStation.TryGetTrainLine() != null)
        {
            line = mSelectedStation.TryGetTrainLine();
            line.AddStation(station);
            return;
        }

        line = Instantiate(mLinePrefab).GetComponent<TrainLine>();
        line.transform.parent = transform;
        line.Initialize(mSelectedStation, station, mIdCounter);

        // Just for Testing make a Train here
        var train = Instantiate(mTrainPrefab).GetComponent<Train>();
        train.transform.parent = line.transform;
        train.SetLine(line);

        mIdCounter++;

        mSelectedStation = null;
    }
}
