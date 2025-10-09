using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public float maxY = 7f;
    public float minY = -2;
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed * Time.fixedDeltaTime;
            transform.LookAt(target.position);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * speed * Time.fixedDeltaTime;
            transform.LookAt(target.position);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * speed * Time.fixedDeltaTime;
            transform.LookAt(target.position);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed * Time.fixedDeltaTime;
            transform.LookAt(target.position);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += transform.up * speed * Time.fixedDeltaTime;
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
            transform.LookAt(target.position);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += -transform.up * speed * Time.fixedDeltaTime;
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
            transform.LookAt(target.position);
        }
    }


    /*
    public InputActionAsset cameraInputAsset;

    public Transform target;

    InputAction ForwardAction;

    void Start()
    {
        
        ForwardAction = cameraInputAsset.FindAction("Forward");
        
        
        if (ForwardAction == null)
        {
            
        }
        ForwardAction.Enable();
        //ForwardAction.performed += OnFr;
        ForwardAction += OnFr;
       
}

public void OnFr(InputAction.CallbackContext context)
    {
        Debug.Log("fsf");
    }

    public void OnForward(InputAction.CallbackContext context)
    {
        context => 
    }

    public void OnBackward()
    {
        transform.position += Vector3.back;
        transform.LookAt(target);
    }

    public void OnLeft()
    {
        transform.position += Vector3.left;
        transform.LookAt(target);
    }

    public void OnRight()
    {
        transform.position += Vector3.right;
        transform.LookAt(target);
    }

    public void OnUp()
    {
        transform.position += Vector3.up;
        transform.LookAt(target);
    }

    public void OnDown()
    {
        transform.position += Vector3.down;
        transform.LookAt(target);
    }
    */
}
