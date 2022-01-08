using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicCameraControls : MonoBehaviour
{
    [SerializeField]
    public float speed = 5.0f;
    private float mouseScrollY;
    private Vector3 movement = new Vector3(0, 0, -10);
    private Vector2 mMoveVec;

    // Start is called before the first frame update
    void Start()
    {
        // set camera movement to 0-vector 2D vector
        // movement = new Vector3(0, 0, -10);
        transform.position = new Vector3(3500, 4100, -10);
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(mMoveVec.x, mMoveVec.y, 0 ) * speed;
    }

    void OnMoveCamera(InputValue input)
    {
        mMoveVec = input.Get<Vector2>() * GetComponent<Camera>().orthographicSize * 0.01f;
    }

    void OnMouseScroll(InputValue input)
    {
        mouseScrollY = input.Get<float>();
        if(mouseScrollY > 0)
        {
            GetComponent<Camera>().orthographicSize /= mouseScrollY * 0.01f;
        }
        if (mouseScrollY < 0)
        {
            GetComponent<Camera>().orthographicSize *= mouseScrollY * -0.01f;
        }
    }
}