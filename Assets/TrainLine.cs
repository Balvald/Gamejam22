using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainLine : MonoBehaviour
{
    private GameObject mStation1;
    private GameObject mStation2;

    // Start is called before the first frame update
    void Start()
    {
        var lr = transform.gameObject.GetComponent<LineRenderer>();
        if (lr == null)
        {
            lr = transform.gameObject.AddComponent<LineRenderer>();
        }
        // TODO: set parameters
        lr.SetPositions(new Vector3[] {
                mStation1.transform.position,
                mStation2.transform.position
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStations(GameObject stat1, GameObject stat2)
    {
        mStation1 = stat1;
        mStation2 = stat2;
    }

    void OnDestroy()
    {
        mStation1?.GetComponent<TrainStation>().RemoveAdjacent(mStation2);
        mStation2?.GetComponent<TrainStation>().RemoveAdjacent(mStation1);
    }
}
