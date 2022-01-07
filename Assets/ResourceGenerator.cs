using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public ResourceType ResourceToGenerate { get; set; }
    public int SecondsToWait { get; set; }

    private Coroutine mCoroutine;

    private ResourceManager mResourceManager;
    public int Efficiency { get; set; }

    private bool mEnabled = true;
    void Start()
    {
        mResourceManager = FindObjectOfType<ResourceManager>();
        mCoroutine = StartCoroutine(GenerateResource());
    }

    private IEnumerator GenerateResource()
    {
        while (mEnabled)
        {
            mResourceManager.AddResource(ResourceToGenerate, Efficiency);

            yield return new WaitForSeconds(SecondsToWait);
        }
    }

    void OnDestroy()
    {
        StopCoroutine(mCoroutine);
    }
}
