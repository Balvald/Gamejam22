using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private Dictionary<ResourceType, int> mResources;

    // Start is called before the first frame update
    void Awake()
    {
        PrepareDictionary();
    }

    private void PrepareDictionary()
    {
        mResources = new Dictionary<ResourceType, int>();
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            mResources[type] = 0;
        }
    }

    public void AddResource(ResourceType type, int amount)
    {
        mResources[type] += amount;
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

    public int GetResourceAmount(ResourceType type)
    {
        return mResources[type];
    }

    public bool PerformBuy(int iron, int coal, int money)
    {
        // TODO: this is not adaptable to more Resources
        if (!HasResource(iron, coal, money))
        {
            return false;
        }

        RemoveResource(ResourceType.Iron, iron);
        RemoveResource(ResourceType.Coal, coal);
        RemoveResource(ResourceType.Money, money);
        return true;
    }
}

public enum ResourceType
{
    Iron,
    Coal,
    Money
}
