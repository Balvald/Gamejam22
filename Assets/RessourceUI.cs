using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RessourceUI : MonoBehaviour
{
    private ResourceManager mResourceManager;

    [SerializeField]
    private Text mIronText;
    [SerializeField]
    private Text mCoalText;
    [SerializeField]
    private Text mMoneyText;

    void Start()
    {
        mResourceManager = FindObjectOfType<ResourceManager>();
        mIronText = transform.Find("IronText").GetComponent<Text>();
        mCoalText = transform.Find("CoalText").GetComponent<Text>();
        mMoneyText = transform.Find("MoneyText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        mIronText.text = mResourceManager.GetResourceAmount(ResourceType.Iron).ToString();
        mCoalText.text = mResourceManager.GetResourceAmount(ResourceType.Coal).ToString();
        mMoneyText.text = mResourceManager.GetResourceAmount(ResourceType.Money).ToString();
    }
}
