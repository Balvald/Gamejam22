using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickManager : MonoBehaviour
{
    private GameObject mObjectUnderMouse;
    [SerializeField]
    private Camera mMainCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentObject();
    }

    private void GetCurrentObject()
    {
        Ray ray = mMainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        IHoverable hov;
        if (Physics.Raycast(ray, out hit) && hit.collider != null)
        {
            if (mObjectUnderMouse == hit.collider.gameObject)
            {
                return;
            }

            mObjectUnderMouse = hit.collider.gameObject;
            mObjectUnderMouse.TryGetComponent<IHoverable>(out hov);
            hov?.OnHoverEnter();
            return;
        }

        if (mObjectUnderMouse != null)
        {
            mObjectUnderMouse.TryGetComponent<IHoverable>(out hov);
            hov?.OnHoverExit();
        }
        mObjectUnderMouse = null;
    }
}
