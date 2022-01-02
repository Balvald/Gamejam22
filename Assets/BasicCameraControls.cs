using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraControls : MonoBehaviour
{
    public float speed = 5.0f;
    private Vector3 movement = new Vector3(0, 0, -10);
    

    // Start is called before the first frame update
    void Start()
    {
        // set camera movement to 0-vector 2D vector
        movement = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement -= new Vector3(0, speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement += new Vector3(0, speed * Time.deltaTime, 0);
        }
        transform.position = movement;
    }
}
