using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceType mResourceToGenerate;

    public ResourceType ResourceToGenerate
    {
        get => mResourceToGenerate;
        set
        {
            mResourceToGenerate = value;
            SetSprite();
        }
    }
    public int SecondsToWait { get; set; }

    private Coroutine mCoroutine;

    private ResourceManager mResourceManager;
    public int Efficiency { get; set; }

    private bool mEnabled = true;
    void Start()
    {
        mResourceManager = FindObjectOfType<ResourceManager>();
        // mCoroutine = StartCoroutine(GenerateResource());
    }

    private IEnumerator GenerateResource()
    {
        while (mEnabled)
        {
            mResourceManager.AddResource(ResourceToGenerate, Efficiency);

            yield return new WaitForSeconds(SecondsToWait);
        }
    }

    public void Produce()
    {
        mResourceManager.AddResource(ResourceToGenerate, Efficiency);
    }

    private void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = ResourceManager.Sprites[ResourceToGenerate];
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
