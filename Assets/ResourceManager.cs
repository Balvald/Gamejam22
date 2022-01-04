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

    void AddResource(ResourceType type, int amount)
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

    public int GetResourceAmount(ResourceType type)
    {
        return mResources[type];
    }
}

public enum ResourceType
{
    Iron,
    Coal,
    Money
}
