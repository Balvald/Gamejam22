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
        Vector2 left;
        Vector2 right;
        if (pos1.x <= pos2.x)
        {
            left = new Vector2(pos1.x, pos1.y);
            right = new Vector2(pos2.x, pos2.y);
        }
        else
        {
            right = new Vector2(pos1.x, pos1.y);
            left = new Vector2(pos2.x, pos2.y);
        }

        path = new Path(left, right);
    }

    public void AddPosition(Vector3 position)
    {
        var pos = new Vector2(position.x,position.y);
        path.AddSegment(pos);
    }
}