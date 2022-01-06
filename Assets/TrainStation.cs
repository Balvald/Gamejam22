using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrainStation : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private HashSet<TrainStation> mAdjacentStations = new HashSet<TrainStation>();

    private Dictionary<int, TrainLine> mConnenctedLines = new Dictionary<int, TrainLine>();

    [SerializeField]
    private GameObject mLineButtonPrefab;

    [SerializeField]
    private int mMaxConnectedLines = 3;

    private bool HasSpaceForLines => mConnenctedLines.Count <= mMaxConnectedLines;

    private LineConstructor mLineConstructor;
    void Start()
    {
        Physics.queriesHitTriggers = true;
        mLineConstructor = FindObjectOfType<LineConstructor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            mLineConstructor.HandleStationClick(this);
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
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
        if (mConnenctedLines.Count >= mMaxConnectedLines)
        {
            return;
        }

        mConnenctedLines[trainLine.ID] = trainLine;
    }

    public void CallConstructor()
    {
        mLineConstructor.HandleStationClick(this);
    }

    public List<Button> GetButtons()
    {
        var btns = new List<Button>();

        if (HasSpaceForLines)
        {
            // create newLine Button

            // get instance
            var o = Instantiate(mLineButtonPrefab);
            var btn = o.GetComponent<Button>();
            var tooltip = o.GetComponent<ToolTipAccessor>();

            tooltip.UpdateToolTipString("start new Line here");

            btn.onClick.AddListener(CallConstructor);

            btns.Add(btn);
        }

        return btns;
    }
}
