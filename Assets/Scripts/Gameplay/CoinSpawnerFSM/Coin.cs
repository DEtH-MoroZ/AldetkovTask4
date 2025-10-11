using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Path;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviourExt
{
    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public bool registered = false;
    private Collider col;
    public GameObject trail;

    [OnAwake]
    private void TheAwake()
    {
        gameObject.SetActive(false);
        trail.SetActive(false);
        col = GetComponent<Collider>();
    }

    public void Spawn()
    {
        transform.localScale = Vector3.one;
        rb.constraints = (RigidbodyConstraints) 10;
        gameObject.SetActive(true);
        col.enabled = true;
    }

    public void Despawn()
    {
        //trail.SetActive(true);
        Model.EventManager.Invoke("PlayCoinPickUpVFX", transform.position);
        Model.EventManager.Invoke("PlayCoinPickUp", transform.position);
        StartCoroutine(DespawnCoroutine());
        Model.EventManager.Invoke("PlayCoinWhoosh", transform.position);
    }
    
    IEnumerator DespawnCoroutine()
    {        

        col.enabled = false;
        rb.constraints = 0;

        float timer = 0;
        Vector3 startPosition = rb.position;
        for (int a = 0; a < 50; a++)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
            rb.Move(
                GetQuadraticBezierPoint(
                startPosition,
                //(rb.position + new Vector3(Model.GetFloat("GathererPositionX"), 4.0f, Model.GetFloat("GathererPositionZ"))) / 2f,
                startPosition + Vector3.up * 10f,
                new Vector3(Model.GetFloat("GathererPositionX"), 2.0f, Model.GetFloat("GathererPositionZ")),
                Mathf.Lerp(0f, 50f, timer/50f)),
                Quaternion.LookRotation(new Vector3(Model.GetFloat("GathererPositionX"), 1.0f, Model.GetFloat("GathererPositionZ")))
            );

            transform.localScale = Vector3.one * (1-Mathf.Lerp(0f, 25f, timer / 50f));
            

        }

        registered = false;
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //trail.SetActive(false);
        yield return null;
    }
    //this actually needs additional testing, i guess. not the best solution
    
    public void OnCollisionEnter(Collision collision)
    {
        if (transform.position.y < 2f && registered == false)
        {
            Model.EventManager.Invoke("PlayCoinDrop", transform.position);
            Model.EventManager.Invoke("AddCoinToGrid", this);
            registered = true;
        }
    }

    public static Vector3 GetQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float uu = u * u;
        float tt = t * t;

        Vector3 p = uu * p0; // (1-t)^2 * P0
        p += 2 * u * t * p1; // 2 * (1-t) * t * P1
        p += tt * p2; // t^2 * P2

        return p;
    }
}
