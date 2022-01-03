using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrainStation : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private HashSet<TrainStation> mAdjacentStations = new HashSet<TrainStation>();

    private Dictionary<int, TrainLine> mConnenctedLines = new Dictionary<int, TrainLine>();
    // Start is called before the first frame update
    void Start()
    {
        Physics.queriesHitTriggers = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Object has been Clicked -> Left");
            var constructor = FindObjectOfType<LineConstructor>();
            constructor.HandleStationClick(this);
            return;
        }
        Debug.Log("Object has been Clicked -> Right");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hovering over Object");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exiting Hover");
    }

    public bool HasAdjacent(TrainStation adj)
    {
        return mAdjacentStations.Contains(adj);
    }

    public void AddAdjacent(TrainStation adj)
    {
        if (adj == this)
        {
            return;
        }
        mAdjacentStations.Add(adj);
    }
    
    public void RemoveAdjacent(TrainStation adj)
    {
        mAdjacentStations.Remove(adj);
    }

    public TrainLine TryGetTrainLine(int id = -1)
    {
        if (mConnenctedLines.Count == 0)
        {
            return null;
        }

        if (id != -1 && mConnenctedLines.ContainsKey(id))
        {
            return mConnenctedLines[id];
        }

        // return just any
        return mConnenctedLines.First().Value;
    }

    public void AddLine(TrainLine trainLine)
    {
        mConnenctedLines[trainLine.ID] = trainLine;
    }
}
