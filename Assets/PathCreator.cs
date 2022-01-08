using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathCreator : MonoBehaviour
{

    [HideInInspector]
    public Path path;

    public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = .1f;
    public float controlDiameter = .075f;
    public bool displayControlPoints = true;

    public void CreatePath()
    {
        path = new Path(transform.position);
    }

    public void CreatePathFromPositions(Vector3 pos1, Vector3 pos2)
    {
        path = new Path(pos1, pos2);
    }

    public void AddPosition(Vector3 position, bool atFront)
    {
        var pos = new Vector2(position.x,position.y);
        if (atFront)
        {
            path.AddSegmentTrainFront(pos);
            return;
        }
        path.AddSegmentTrain(pos);
    }
}