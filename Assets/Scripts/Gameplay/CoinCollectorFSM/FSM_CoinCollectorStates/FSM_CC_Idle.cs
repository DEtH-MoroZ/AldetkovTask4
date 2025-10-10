using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_CC_Idle")]
public class FSM_CC_Idle : FSMState
{
    [Enter]
    private void OnEnter()
    {
        Model.Set("GathererInputX", 0.0f);
        Model.Set("GathererInputY", 0.0f);

        Model.Set("BtnStartButtonEnable", true);
        Model.Set("BtnStopButtonEnable", false);

        Model.Set("GOCharacterTargetEnable", false);

        Debug.Log("FSM_CC_Idle");
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
}
