using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviourExt
{
    public Rigidbody rb;

    [OnAwake]
    private void awake()
    {
        gameObject.SetActive(false);
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        enabled = true;
                
    }


    public void Despawn()
    {
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    
    //this actually needs additional testing, i guess. not the best solution
    /*
    public void OnCollisionEnter(Collision collision)
    {
            Model.EventManager.Invoke("AddCoinToGrid", this);
            if (transform.position.y > 3f)
            {
            return;
            }
        
            
            enabled = false;
        
    }
    */
    /*
    public void FixedUpdate()
    {
        if (transform.position.y < 1.8f)
        {
            Model.EventManager.Invoke("AddCoinToGrid", this);
            enabled = false;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Model.EventManager.Invoke("AddCoinToGrid", this);
            enabled = false;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Model.EventManager.Invoke("AddCoinToGrid", this);
            enabled = false;
        }
    }*/
}
