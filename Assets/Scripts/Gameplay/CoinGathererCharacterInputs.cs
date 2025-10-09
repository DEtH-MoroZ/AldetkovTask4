using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CoinGathererCharacterInputs : MonoBehaviourExtBind
{
    public Transform target;

    private StarterAssetsInputs _starterAssetsInputs;

    private Vector3 direction;

    private float InputX = 0.0f;
    private float InputY = 0.0f;

    private Collider otherCollider;

    //[OnStart]
    //private void start() //this one doesnt initialize
    [OnAwake]
    private void awake()
    {
        _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    [Bind("OnGathererInputXChanged")]
    [Bind("OnGathererInputYChanged")]
    private void OnGathererInputChanged()
    {
        InputX = Model.GetFloat("GathererInputX");
        InputY = Model.GetFloat("GathererInputY");
        _starterAssetsInputs.move = new Vector2(InputX, InputY);
    }

    [Bind("OnTargetXChanged")]
    [Bind("OnTargetZChanged")]
    private void OnTargetChanged()
    {        
        target.transform.position = new Vector3(Model.GetFloat("TargetX"), 0.0f, Model.GetFloat("TargetZ"));
    }


    private void FixedUpdate()
    {
        Model.Set("GathererPositionX", transform.position.x);
        Model.Set("GathererPositionZ", transform.position.z);
        
    }
    [Bind("OnRunModeChanged")]
    private void RunModeSwitch(bool newMode)
    {
        _starterAssetsInputs.sprint = newMode;
    }
}
