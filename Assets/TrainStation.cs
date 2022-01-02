using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrainStation : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.queriesHitTriggers = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Object has been Clicked -> Left");
            return;
        }
        Debug.Log("Object has been Clicked -> Right");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hovering over Object");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exiting Hover");
    }
}
