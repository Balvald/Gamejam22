using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConstructor : MonoBehaviour
{
    private GameObject mStartStation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mStartStation == null)
        {
            return;
        }

        // TODO: render preview
    }

    public void HandleStationClick(GameObject station)
    {
        if (mStartStation == null)
        {
            mStartStation = station;
            return;
        }

        // TODO: switch to instantiating Prefab
        var newlineObj = new GameObject();
        var newline = newlineObj.AddComponent<TrainLine>();
        newline.SetStations(mStartStation, station);
        newlineObj.transform.parent = transform;
    }
}
