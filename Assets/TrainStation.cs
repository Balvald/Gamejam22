 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TrainStation : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private HashSet<TrainStation> mAdjacentStations = new HashSet<TrainStation>();

    private Dictionary<int, TrainLine> mConnenctedLines = new Dictionary<int, TrainLine>();

    public int stationDataIndex;

    [SerializeField]
    private GameObject mLineButtonPrefab;

    [SerializeField]
    private int mMaxConnectedLines = 3;

    private bool HasSpaceForLines => mConnenctedLines.Count <= mMaxConnectedLines;

    private LineConstructor mLineConstructor;

    // -------------------------------------- Resources
    [SerializeField]
    private GameObject mGeneratorPrefab;

    private Dictionary<ResourceType, ResourceGenerator> mResourceGenerators = new Dictionary<ResourceType, ResourceGenerator>();

    public ResourceEfficiency mResourceEfficiency = ResourceEfficiency.Default;

    private ResourceManager mResourceManager;

    [SerializeField]
    private Material mDefaultMaterial;

    [SerializeField]
    private Material mSelectedMaterial;

    void Start()
    {
        Physics.queriesHitTriggers = true;
        mLineConstructor = FindObjectOfType<LineConstructor>();
        mResourceManager = FindObjectOfType<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && mLineConstructor.HasSelected)
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

    public void CallConstructor(int id  = -1)
    {
        mLineConstructor.HandleStationClick(this, id);
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

            tooltip.UpdateToolTipString("start new Line here" + "\n" + mResourceManager.NewlineCost);

            btn.onClick.AddListener(delegate{
                if (mResourceManager.PerformBuy())
                {
                    CallConstructor();
                }
            });

            btns.Add(btn);
        }

        foreach (var line in mConnenctedLines)
        {
            if (line.Value.IsLastStation(this))
            {
                var o = Instantiate(mLineButtonPrefab);
                var btn = o.GetComponent<Button>();
                var tooltip = o.GetComponent<ToolTipAccessor>();

                tooltip.UpdateToolTipString("continue Line " + line.Key + "\n" + mResourceManager.NewlineCost);

                btn.onClick.AddListener(delegate{
                    if (mResourceManager.PerformBuy())
                    {
                        CallConstructor(line.Key);
                    }
                });

                btns.Add(btn);
            }
        }

        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            var btn = GetResourceButton(type);
            if (btn != null)
            {
                btns.Add(btn);
            }
        }

        return btns;
    }

    private Button GetResourceButton(ResourceType type)
    {
        if (mResourceGenerators.ContainsKey(type))
        {
            return null;
        }

        var eff = mResourceEfficiency[type];
        if (eff <= 0)
        {
            return null;
        }
        var o = Instantiate(mLineButtonPrefab);
        var btn = o.GetComponent<Button>();
        var tooltip = o.GetComponent<ToolTipAccessor>();
        var image = o.GetComponent<Image>();

        var tooltipString = "Build " + type + " Production\n " + mResourceManager.ProducerCosts[type].ToString();

        tooltip.UpdateToolTipString(tooltipString);

        btn.onClick.AddListener(delegate{
            if (mResourceManager.PerformBuy(type))
            {
                AddProducer(type);
            }
        });

        image.sprite = ResourceManager.Sprites[type];

        return btn;
    }

    public void AddProducer(ResourceType type)
    {
        var o = Instantiate(mGeneratorPrefab);
        var gen = o.GetComponent<ResourceGenerator>();

        gen.ResourceToGenerate = type;
        gen.Efficiency = mResourceEfficiency[type];
        gen.SecondsToWait = mResourceEfficiency.Speed;

        o.transform.SetParent(transform);

        o.transform.localPosition = Quaternion.AngleAxis(mResourceGenerators.Keys.Count * 120, Vector3.forward) * (Vector3.up * 3);

        mResourceGenerators[type] = gen;
    }

    public int GetStationDataIndex()
    {
        return stationDataIndex;
    }

    public void NotifyTrainPassing()
    {
        foreach (var gen in mResourceGenerators)
        {
            gen.Value.Produce();
        }
    }
    public void SetResourceEfficiency(ResourceEfficiency resEff)
    {
        mResourceEfficiency = resEff;
    }

    public int GetResourceEfficiency(ResourceType type)
    {
        return mResourceEfficiency.Efficiencies[type];
    }

    public void SelectStation()
    {
        GetComponent<SpriteRenderer>().material = mSelectedMaterial;
    }

    public void DeselectStation()
    {
        GetComponent<SpriteRenderer>().material = mDefaultMaterial;
    }
}

public struct ResourceEfficiency
{
    public Dictionary<ResourceType, int> Efficiencies;

    public int this[ResourceType type] => Efficiencies[type];

    public int Speed;

    public static ResourceEfficiency Default = new ResourceEfficiency(10,10,10,2);

    public int Iron => Efficiencies[ResourceType.Iron];
    public int Coal => Efficiencies[ResourceType.Coal];
    public int Money => Efficiencies[ResourceType.Money];
    public ResourceEfficiency(int iron, int coal, int money, int speed = 3)
    {
        Efficiencies = new Dictionary<ResourceType, int>();
        Efficiencies[ResourceType.Iron] = iron;
        Efficiencies[ResourceType.Coal] = coal;
        Efficiencies[ResourceType.Money] = money;

        Speed = speed;
    }

    public override string ToString()
    {
        var str = "Potential: \n" +
                  (Iron != 0 ? "Iron: " + Iron + "\n" : "") +
                  (Coal != 0 ? "Coal: " + Coal + "\n" : "") +
                  (Money != 0 ? "Money: " + Money : "");
        return str;
    }
}
