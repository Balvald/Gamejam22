using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[System.Serializable]
public class Path
{

    private float mDistanceThreshold = 0.3f;

    [SerializeField, HideInInspector]
    List<Vector2> points;
    [SerializeField, HideInInspector]
    bool isClosed;
    [SerializeField, HideInInspector]
    bool autoSetControlPoints;

    private List<float> mSegmentLength = new List<float>();

    public Path(Vector2 centre)
    {
        points = new List<Vector2>
        {
            centre+Vector2.left,
            centre+(Vector2.left+Vector2.up)*.5f,
            centre + (Vector2.right+Vector2.down)*.5f,
            centre + Vector2.right
        };
    }
    public Path(Vector2 pos1, Vector2 pos2)
    {
        points = new List<Vector2>();
        points.Add(pos1);
        AddSegmentTrain(pos1,pos2);
    }

    public Vector2 this[int i] => points[i];

    public bool IsClosed
    {
        get
        {
            return isClosed;
        }
        set
        {
            if (isClosed != value)
            {
                isClosed = value;

                if (isClosed)
                {
                    points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
                    points.Add(points[0] * 2 - points[1]);
                    if (autoSetControlPoints)
                    {
                        AutoSetAnchorControlPoints(0);
                        AutoSetAnchorControlPoints(points.Count - 3);
                    }
                }
                else
                {
                    points.RemoveRange(points.Count - 2, 2);
                    if (autoSetControlPoints)
                    {
                        AutoSetStartAndEndControls();
                    }
                }
            }
        }
    }

    public bool AutoSetControlPoints
    {
        get
        {
            return autoSetControlPoints;
        }
        set
        {
            if (autoSetControlPoints != value)
            {
                autoSetControlPoints = value;
                if (autoSetControlPoints)
                {
                    AutoSetAllControlPoints();
                }
            }
        }
    }

    public int NumPoints => points.Count;
    public int NumSegments => points.Count / 3;

    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPos) * .5f);
        points.Add(anchorPos);

        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(points.Count - 1);
        }
    }

    public void AddSegmentTrainFront(Vector2 station)
    {
        var p = GetNewPoints(station, points.First());

        points.InsertRange(0, new []{station, p.Item1, p.Item2});

        mSegmentLength.Insert(0, GetEstSegmentLength(0));

    }

    public void AddSegmentTrain(Vector2 newStation)
    {
        AddSegmentTrain(points.Last(), newStation);
    }

    public void AddSegmentTrain(Vector2 oldStation, Vector2 newStation)
    {
        var p = GetNewPoints(oldStation, newStation);

        points.Add(p.Item1);
        points.Add(p.Item2);
        points.Add(newStation);

        mSegmentLength.Add(GetEstSegmentLength(NumSegments - 1));
    }

    public (Vector2, Vector2) GetNewPoints(Vector2 oldStation, Vector2 newStation)
    {
        var distances = newStation - oldStation;

        var distx = Mathf.Abs(oldStation.x - newStation.x);
        var disty = Mathf.Abs(oldStation.y - newStation.y);

        var maxi = Mathf.Max(disty, distx);
        var mini = Mathf.Min(distx, disty);

        var relation = mini / maxi;

        Vector2 p1, p2;

        if (relation <= mDistanceThreshold)
        {
            p1 = oldStation + ((disty >= distx) ? (distances.y / 2) * Vector2.up : (distances.x / 2) * Vector2.right);
            p2 = newStation - ((disty >= distx) ? (distances.y / 2) * Vector2.up : (distances.x / 2) * Vector2.right);
        }
        else
        {
            p1 = oldStation + distances.y * Vector2.up;
            p2 = newStation - distances.x * Vector2.right;
        }

        return (p1, p2);
    }

    public void SplitSegment(Vector2 anchorPos, int segmentIndex)
    {
        points.InsertRange(segmentIndex * 3 + 2, new Vector2[] { Vector2.zero, anchorPos, Vector2.zero });
        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
        }
        else
        {
            AutoSetAnchorControlPoints(segmentIndex * 3 + 3);
        }
    }

    public void DeleteSegment(int anchorIndex)
    {
        if (NumSegments > 2 || !isClosed && NumSegments > 1)
        {
            if (anchorIndex == 0)
            {
                if (isClosed)
                {
                    points[points.Count - 1] = points[2];
                }
                points.RemoveRange(0, 3);
            }
            else if (anchorIndex == points.Count - 1 && !isClosed)
            {
                points.RemoveRange(anchorIndex - 2, 3);
            }
            else
            {
                points.RemoveRange(anchorIndex - 1, 3);
            }
        }
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        return new Vector2[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[LoopIndex(i * 3 + 3)] };
    }

    public Vector2 GetPositionInSegment(int segment, float t)
    {
        var p = GetPointsInSegment(segment);
        return Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
    }

    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i];

        if (i % 3 == 0 || !autoSetControlPoints)
        {
            points[i] = pos;

            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(i);
            }
            else
            {

                if (i % 3 == 0)
                {
                    if (i + 1 < points.Count || isClosed)
                    {
                        points[LoopIndex(i + 1)] += deltaMove;
                    }
                    if (i - 1 >= 0 || isClosed)
                    {
                        points[LoopIndex(i - 1)] += deltaMove;
                    }
                }
                else
                {
                    bool nextPointIsAnchor = (i + 1) % 3 == 0;
                    int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
                    int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

                    if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                    {
                        float dst = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                        Vector2 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                        points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * dst;
                    }
                }
            }
        }
    }

    public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
    {
        List<Vector2> evenlySpacedPoints = new List<Vector2>();
        evenlySpacedPoints.Add(points[0]);
        Vector2 previousPoint = points[0];
        float dstSinceLastEvenPoint = 0;

        for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
        {
            Vector2[] p = GetPointsInSegment(segmentIndex);
            float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
            float estimatedCurveLength = Vector2.Distance(p[0], p[3]) + controlNetLength / 2f;
            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
            float t = 0;
            while (t <= 1)
            {
                t += 1f / divisions;
                Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                dstSinceLastEvenPoint += Vector2.Distance(previousPoint, pointOnCurve);

                while (dstSinceLastEvenPoint >= spacing)
                {
                    float overshootDst = dstSinceLastEvenPoint - spacing;
                    Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                    evenlySpacedPoints.Add(newEvenlySpacedPoint);
                    dstSinceLastEvenPoint = overshootDst;
                    previousPoint = newEvenlySpacedPoint;
                }

                previousPoint = pointOnCurve;
            }
        }

        return evenlySpacedPoints.ToArray();
    }


    void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
        {
            if (i >= 0 && i < points.Count || isClosed)
            {
                AutoSetAnchorControlPoints(LoopIndex(i));
            }
        }

        AutoSetStartAndEndControls();
    }

    void AutoSetAllControlPoints()
    {
        for (int i = 0; i < points.Count; i += 3)
        {
            AutoSetAnchorControlPoints(i);
        }

        AutoSetStartAndEndControls();
    }

    void AutoSetAnchorControlPoints(int anchorIndex)
    {
        Vector2 anchorPos = points[anchorIndex];
        Vector2 dir = Vector2.zero;
        float[] neighbourDistances = new float[2];

        if (anchorIndex - 3 >= 0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex - 3)] - anchorPos;
            dir += offset.normalized;
            neighbourDistances[0] = offset.magnitude;
        }
        if (anchorIndex + 3 >= 0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex + 3)] - anchorPos;
            dir -= offset.normalized;
            neighbourDistances[1] = -offset.magnitude;
        }

        dir.Normalize();

        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
            {
                points[LoopIndex(controlIndex)] = anchorPos + dir * neighbourDistances[i] * .5f;
            }
        }
    }

    void AutoSetStartAndEndControls()
    {
        if (!isClosed)
        {
            points[1] = (points[0] + points[2]) * .5f;
            points[points.Count - 2] = (points[points.Count - 1] + points[points.Count - 3]) * .5f;
        }
    }

    int LoopIndex(int i)
    {
        return (i + points.Count) % points.Count;
    }

    private float GetEstSegmentLength(int i)
    {
        var p = GetPointsInSegment(i);
        float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
        var estimatedCurveLength = Vector2.Distance(p[0], p[3]) + controlNetLength / 2f;
        return estimatedCurveLength;
    }

    public float GetLengthOfSegment(int i)
    {
        return mSegmentLength[i];
    }

}