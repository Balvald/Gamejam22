using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceGeneration : MonoBehaviour
{
    public GameObject myPrefab;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
