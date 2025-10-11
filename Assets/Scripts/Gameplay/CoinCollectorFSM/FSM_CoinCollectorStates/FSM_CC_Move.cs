using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_CC_Move")]
public class FSM_CC_Move : FSMState
{
    [Enter]
    private void OnEnter()
    {
        
        newDirection();

        Model.Set("BtnStartButtonEnable", false);
        Model.Set("BtnStopButtonEnable", true);

        Model.Set("GOCharacterTargetEnable", true);

        Debug.Log("FSM_CC_Move");
    }

    private void newDirection()
    {
        Vector2 vec = new Vector2(Model.GetFloat("TargetX") - Model.GetFloat("GathererPositionX"), Model.GetFloat("TargetZ") - Model.GetFloat("GathererPositionZ"));
        vec = vec.normalized;

        Model.Set("GathererInputX", vec.x);
        Model.Set("GathererInputY", vec.y);
    }

    [Loop(0.10f)]
    private void CheckProxymityAndDespawn()
    {

        Model.EventManager.Invoke(
            "CheckProxymityAndDespawn",
            new Vector3(
                Model.GetFloat("GathererPositionX"),
                0.0f,
                Model.GetFloat("GathererPositionZ")
                )
            );
        

    }

    [One(1.5f)]
    private void OnCantReachTarget()
    {
        Parent.Change("FSM_CC_FindTarget");
    }

    /*
    [Loop(0.05f)]
    private void TargetReached()
    {
        Debug.Log("rewrite this");
        Vector2 tart = new Vector2(Model.GetFloat("TargetX"), Model.GetFloat("TargetZ"));
        Vector2 posss = new Vector2(Model.GetFloat("GathererPositionX"), Model.GetFloat("GathererPositionZ"));

        if ( Vector2.Distance(tart,posss) < 0.3f)
        {
            Parent.Change("FSM_CC_FindTarget");
        }
    }*/
  
}
