using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Canvas mCanvas;
    // Start is called before the first frame update
    void Start()
    {
        mCanvas = GetComponentInChildren<Canvas>();
        mCanvas.gameObject.SetActive(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        mCanvas.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mCanvas.gameObject.SetActive(false);
    }
}
