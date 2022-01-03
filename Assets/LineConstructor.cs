using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConstructor : MonoBehaviour
{
    private TrainStation mStartStation;

    [SerializeField]
    private GameObject myPrefab;

    private int mIdCounter;

    // Update is called once per frame
    void Update()
    {
        if (mStartStation == null)
        {
            return;
        }

        // TODO: render preview
    }

    public void HandleStationClick(TrainStation station)
    {
        if (mStartStation == null)
        {
            mStartStation = station;
            return;
        }

        if (station == mStartStation || station.GetComponent<TrainStation>().HasAdjacent(mStartStation))
        {
            return;
        }

        TrainLine line;
        if (mStartStation.TryGetTrainLine() != null)
        {
            line = mStartStation.TryGetTrainLine();
            line.AddStation(station);
            return;
        }

        line = Instantiate(myPrefab).GetComponent<TrainLine>();
        line.transform.parent = transform;
        line.Initialize(mStartStation, station, mIdCounter);
        mIdCounter++;

        mStartStation = null;
    }
}
