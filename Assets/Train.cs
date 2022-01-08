using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    private TrainLine mAssignedTrainLine;
    private Vector3 mLastPosition;

    [SerializeField]
    private float mSpeed = 1;

    public int LastSegment { get; set; }

    public float CurrentPosition { get; set; }

    [SerializeField]
    public TrainMovement mMovement;

    void Start()
    {
        mMovement = TrainMovement.Forwards;
        mLastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (mMovement == TrainMovement.Idle)
        {
            return;
        }

        MoveTrain();
    }

    public void SetLine(TrainLine line)
    {
        mAssignedTrainLine = line;
    }

    private void MoveTrain()
    {
        var path = mAssignedTrainLine.GetPath();

        var currentSegment = Mathf.Max(Mathf.Min(path.NumSegments - 1, Mathf.FloorToInt(CurrentPosition)),0);
        var currSegLength = path.GetLengthOfSegment(currentSegment);

        mLastPosition = transform.position;
        CurrentPosition += mSpeed/currSegLength * Time.deltaTime * (mMovement == TrainMovement.Forwards ? 1 : -1);

        var t = CurrentPosition - currentSegment;

        // TODO: this is horrible to read...

        var inNewSegment = LastSegment != currentSegment;
        var atLastStation = currentSegment == path.NumSegments - 1 && t >= 1;
        var atFirstStation = currentSegment == 0 && t <= 0;

        if (LastSegment != currentSegment)
        {
            LastSegment = currentSegment;
            StartCoroutine(WaitAtStation(currentSegment));
            return;
        }

        if (currentSegment == path.NumSegments-1 && t >= 1)
        {
            mMovement = TrainMovement.Backwards;
            t = 1;
            StartCoroutine(WaitAtStation(currentSegment + 1));
        }

        if (currentSegment == 0 && t <= 0)
        {
            mMovement = TrainMovement.Forwards;
            t = 0;
            StartCoroutine(WaitAtStation(0));
        }

        var pos = path.GetPositionInSegment(currentSegment, t);
        transform.position = new Vector3(pos.x,pos.y,0);

        // look to the front
        Vector3 relativePos = transform.position - mLastPosition;
        Quaternion rotation = Quaternion.LookRotation(relativePos) * (mMovement == TrainMovement.Backwards
                                  ? Quaternion.identity
                                  : Quaternion.Euler(0,
                                      0,
                                      180));
        rotation.x = transform.rotation.x;
        rotation.y = transform.rotation.y;
        transform.rotation = rotation;

    }

    private IEnumerator WaitAtStation(int stationIndex)
    {
        var currentMovement = mMovement;
        mMovement = TrainMovement.Idle;

        yield return new WaitForSeconds(2);

        mMovement = currentMovement;

        mAssignedTrainLine.NotifyStation(stationIndex);
    }

    public enum TrainMovement
    {
        Forwards,
        Backwards,
        Idle
    }
}
