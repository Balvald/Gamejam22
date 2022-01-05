using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResourceManager))]
public class ResourceManagerEditor : Editor
{
    private ResourceManager mResourceManager;
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Get Debug Resources"))
        {
            mResourceManager.AddResource(ResourceType.Iron, 1000);
            mResourceManager.AddResource(ResourceType.Coal, 1000);
            mResourceManager.AddResource(ResourceType.Money, 1000);
        }
    }

    void OnEnable()
    {
        mResourceManager = (ResourceManager) target;
    }
}
