using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private Dictionary<ResourceType, int> mResources;

    public static Dictionary<ResourceType, Sprite> Sprites { private set; get; }

    private Dictionary<ResourceType, Cost> ProducerCosts { set; get; }

    private Cost NewlineCost { set; get; }

    // Start is called before the first frame update
    void Awake()
    {
        PrepareDictionary();
        SetStartingResources();
    }

    private void SetStartingResources()
    {
        mResources[ResourceType.Iron] = 300;
        mResources[ResourceType.Coal] = 300;
        mResources[ResourceType.Money] = 700;
    }

    private void PrepareDictionary()
    {
        mResources = new Dictionary<ResourceType, int>();
        Sprites = new Dictionary<ResourceType, Sprite>();
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            mResources[type] = 0;
            Sprites[type] = Resources.Load<Sprite>(type.ToString());
        }

        ProducerCosts = new Dictionary<ResourceType, Cost>();
        ProducerCosts[ResourceType.Iron] = new Cost(20, 70, 100);
        ProducerCosts[ResourceType.Coal] = new Cost(70, 10, 100);
        ProducerCosts[ResourceType.Money] = new Cost(50, 50, 200);

        NewlineCost = new Cost(50, 50, 50);
    }

    public void AddResource(ResourceType type, int amount)
    {
        mResources[type] += amount;
    }

    public void SetResource(ResourceType type, int amount)
    {
        mResources[type] = amount;
    }

    void RemoveResource(ResourceType type, int amount)
    {
        mResources[type] = Mathf.Max(mResources[type] - amount, 0);
    }

    public bool HasResource(ResourceType type, int amount)
    {
        return mResources[type] >= amount;
    }

    public bool HasResource(int iron, int coal, int money)
    {
        return HasResource(ResourceType.Iron, iron) && HasResource(ResourceType.Coal, coal) &&
               HasResource(ResourceType.Money, money);
    }

    public bool HasResource(Cost cost)
    {
        return HasResource(cost.Iron, cost.Coal, cost.Money);
    }

    public int GetResourceAmount(ResourceType type)
    {
        return mResources[type];
    }

    public bool PerformBuy(int iron, int coal, int money)
    {
        // TODO: this is not adaptable to more Resources
        if (!HasResource(iron, coal, money))
        {
            var message = "Not enough ";
            message += !HasResource(ResourceType.Iron, iron) ? "Iron " : "";
            message += !HasResource(ResourceType.Coal, coal) ? " Coal" : "";
            message += !HasResource(ResourceType.Money, money) ? " Money" : "";
            MessageBoard.SendMessage(message, Color.red);
            return false;
        }

        RemoveResource(ResourceType.Iron, iron);
        RemoveResource(ResourceType.Coal, coal);
        RemoveResource(ResourceType.Money, money);
        return true;
    }

    public bool PerformBuy(ResourceType type)
    {
        var c = ProducerCosts[type];
        return PerformBuy(c.Iron, c.Coal, c.Money);
    }

    public bool PerformBuy()
    {
        return PerformBuy(NewlineCost.Iron, NewlineCost.Coal, NewlineCost.Money);
    }
}

public struct Cost
{
    private Dictionary<ResourceType, int> mCosts;

    public int this[ResourceType type] => mCosts[type];

    public int Iron => mCosts[ResourceType.Iron];
    public int Coal=> mCosts[ResourceType.Coal];
    public int Money => mCosts[ResourceType.Money];
    public Cost(int iron , int coal, int money)
    {
        mCosts = new Dictionary<ResourceType, int>();
        mCosts[ResourceType.Iron] = iron;
        mCosts[ResourceType.Coal] = coal;
        mCosts[ResourceType.Money] = coal;
    }
}

public enum ResourceType
{
    Iron,
    Coal,
    Money
}
