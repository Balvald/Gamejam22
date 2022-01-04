using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipAccessor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private string mTooltipString = "";


    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTip.ShowTooltip_Static(mTooltipString);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTip.HideToolTip_Static();
    }
}
