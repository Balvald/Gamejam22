using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PathCreator))]
public class TrainLine : MonoBehaviour
{
    private List<TrainStation> mStations;

    private PathCreator mPathCreator;

    private RoadCreator mRoadCreator;

    private int mIdentifier;

    public int ID => mIdentifier;

    // Start is called before the first frame update
    void Awake()
    {
        mPathCreator = GetComponent<PathCreator>();
        mRoadCreator = GetComponent<RoadCreator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void MoveTrain(Train train)
    {
    }

    public void Initialize(TrainStation stat1, TrainStation stat2, int id)
    {
        if (mStations != null)
        {
            return;
        }

        mStations = new List<TrainStation>();
        mStations.Add(stat1);
        mStations.Add(stat2);

        stat1.AddAdjacent(stat2);
        stat2.AddAdjacent(stat1);

        stat1.AddLine(this);
        stat2.AddLine(this);

        mPathCreator.CreatePathFromPositions(stat1.transform.position, stat2.transform.position);

        mRoadCreator.UpdateRoad();
    }

    public void AddStation(TrainStation station)
    {
        var last = mStations.Last();
        mStations.Add(station);

        last.AddAdjacent(station);
        station.AddAdjacent(last);

        station.AddLine(this);
        mPathCreator.AddPosition(station.transform.position);

        mRoadCreator.UpdateRoad();
    }


    public Path GetPath()
    {
        return mPathCreator.path;
    }
}
