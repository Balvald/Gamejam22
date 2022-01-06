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

    public void SetObjectToUnlock(GameObject gameObject)
    {
        // On game start I'll initialise most stations as unlockable stations
        // For ease of navigating the hierarchy the station to unlock should be a child of the Paywall until the Paywall is removed.
        mObjectToUnlock = gameObject;
        //gameObject.transform.SetParent(transform, false);
        // If this may not have happened
        mObjectToUnlock?.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!mResourceManager.PerformBuy(0,0,mPrice))
        {
            return;
        }

        mObjectToUnlock?.SetActive(true);
        // Deparenting the station otherwise we lose it after Destroying the paywall.
        mObjectToUnlock.transform.parent = null;
        Destroy(gameObject);
    }
}
