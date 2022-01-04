using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    private TrainLine mAssignedTrainLine;
    private Vector3 mLastPosition;

    [SerializeField]
    private float mSpeed = 1;

    private int mCurrentSegment;

    private float mCurrentPosition;

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

        mLastPosition = transform.position;
        mCurrentPosition += mSpeed * Time.deltaTime * (mMovement == TrainMovement.Forwards ? 1 : -1);

        MoveTrain();
    }

    public void SetLine(TrainLine line)
    {
        mAssignedTrainLine = line;
    }

    private void MoveTrain()
    {
        var path = mAssignedTrainLine.GetPath();

        var currentSegment = Mathf.Max(Mathf.Min(path.NumSegments - 1, Mathf.FloorToInt(mCurrentPosition)),0);
        var t = mCurrentPosition - currentSegment;
        if (currentSegment == path.NumSegments-1 && t >= 1)
        {
            mMovement = TrainMovement.Backwards;
            t = 1;
        }

        if (currentSegment == 0 && t < 0)
        {
            mMovement = TrainMovement.Forwards;
            t = 0;
        }

        var pos = path.GetPositionInSegment(currentSegment, t);
        transform.position = new Vector3(pos.x,pos.y,0);

        // look to the front
        Vector3 relativePos = transform.position - mLastPosition;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        rotation.x = transform.rotation.x;
        rotation.y = transform.rotation.y;
        transform.rotation = rotation;
    }

    public enum TrainMovement
    {
        Forwards,
        Backwards,
        Idle
    }
}
