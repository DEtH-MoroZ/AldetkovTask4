using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviourExt
{
    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public bool registered = false;

    [OnAwake]
    private void TheAwake()
    {
        gameObject.SetActive(false);
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }


    public void Despawn()
    {
        registered = false;
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    
    //this actually needs additional testing, i guess. not the best solution
    
    public void OnCollisionEnter(Collision collision)
    {
        if (transform.position.y < 2f && registered == false)
        {
            Model.EventManager.Invoke("AddCoinToGrid", this);
            registered = true;
        }
    }
}
