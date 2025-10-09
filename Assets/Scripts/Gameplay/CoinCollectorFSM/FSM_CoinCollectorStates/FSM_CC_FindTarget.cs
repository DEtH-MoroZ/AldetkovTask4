using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_CC_FindTarget")]
public class FSM_CC_FindTarget : FSMState
{
    [Enter]
    void OnEnter () 
    {
        Model.EventManager.AddAction("TargetFound", TargetFound);
        Model.EventManager.AddAction("TargetNotFound", TargetNotFound);

        Debug.Log("FSM_CC_FindTarget");

        Model.GetFloat("GathererInputX", 0f);
        Model.GetFloat("GathererInputY", 0f);

        Settings.Invoke("FindNewTarget", 
            new Vector3(
                Model.GetFloat("GathererPositionX"),
                0.0f,
                Model.GetFloat("GathererPositionZ")                
                )
            );
    }
    private void TargetFound()
    {
        Debug.Log("[FSM_CC] Target found.");
        Parent.Change("FSM_CC_Move");
    }

    private void TargetNotFound()
    {
        Debug.Log("[FSM_CC] Target not found.");
        Parent.Change("FSM_CC_Idle");
    }

    [OnDestroy]
    void onDestroy ()
    {
        Settings.Model.EventManager.RemoveAction("TargetFound", TargetFound);
        Settings.Model.EventManager.RemoveAction("TargetNotFound", TargetNotFound);
    }
}
