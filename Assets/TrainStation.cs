using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainStation : MonoBehaviour
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

    void OnMouseEnter()
    {
        Debug.Log("entered hover");
    }

    void OnMouseExit()
    {
        Debug.Log("exited hover");
    }

    public void OnStartClick()
    {
        throw new System.NotImplementedException();
    }

    public void OnEndClick()
    {
        throw new System.NotImplementedException();
    }
}
