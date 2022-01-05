using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PayWall : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject mObjectToUnlock;

    private ResourceManager mResourceManager;

    [SerializeField]
    private int mPrice = 10;
    void Start()
    {
        mObjectToUnlock?.SetActive(false);
        mResourceManager = FindObjectOfType<ResourceManager>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!mResourceManager.PerformBuy(0,0,mPrice))
        {
            return;
        }

        mObjectToUnlock?.SetActive(true);
        Destroy(gameObject);
    }
}
