using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Canvas mCanvas;

    [SerializeField]
    private TrainStation mTrainStation;

    private List<Button> mButtons;

    [SerializeField]
    private float mAnimationSpeed = 0.8f;

    [SerializeField]
    private float mDistance = 1;

    private Coroutine mAnimationCoroutine;
    // Start is called before the first frame update


    void Start()
    {
        mCanvas = GetComponentInChildren<Canvas>();
        mButtons = new List<Button>();
        mCanvas.worldCamera = Camera.main;
        mCanvas.gameObject.SetActive(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateButtons();
        mCanvas.transform.position = transform.position;
        mCanvas.gameObject.SetActive(true);
    }

    private void UpdateButtons()
    {
        mButtons = mTrainStation.GetButtons();
        foreach (var btn in mButtons)
        {
            btn.transform.SetParent(mCanvas.transform);
            btn.transform.position = Vector3.zero;
            btn.onClick.AddListener(delegate
            {
                if (mAnimationCoroutine != null)
                {
                    StopCoroutine(mAnimationCoroutine);
                }
                KillButtons();
                UpdateButtons();
                mAnimationCoroutine=StartCoroutine(AnimateButtons());
            });
        }

        mAnimationCoroutine = StartCoroutine(AnimateButtons());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mCanvas.gameObject.SetActive(false);
        StopCoroutine(mAnimationCoroutine);
        ToolTip.HideToolTip_Static(); // just in case
        KillButtons();
    }

    private void KillButtons()
    {
        foreach (var btn in mButtons)
        {
            btn.onClick.RemoveAllListeners();
            Destroy(btn.gameObject);
        }
    }

    private IEnumerator AnimateButtons()
    {
        var numBtn = mButtons.Count;
        if (numBtn <= 0)
        {
            yield break;
        }
        var angle = 360 / numBtn;

        var t = 0.3f;

        while (t < 1)
        {
            t += mAnimationSpeed * Time.deltaTime;
            for (int i = 0; i < numBtn; i++)
            {
                var btn = mButtons[i];
                var desiredPosition = transform.position + Quaternion.AngleAxis(-angle * i, Vector3.forward) * Vector3.up * mDistance;
                //var desiredPosition = Quaternion.AngleAxis(-angle * i, Vector3.forward) * Vector3.up * mDistance;
                t = Mathf.Min(1, t);
                btn.transform.position = Vector3.Lerp(transform.position, desiredPosition, t);
                //btn.transform.position = Vector3.Lerp(Vector3.zero, desiredPosition, t);
            }
            yield return null;
        }
    }
}
