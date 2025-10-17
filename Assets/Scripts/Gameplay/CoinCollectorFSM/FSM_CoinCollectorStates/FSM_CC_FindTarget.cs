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
        Debug.Log("FSM_CC_FindTarget");

        Model.Set("GathererInputX", 0f);
        Model.Set("GathererInputY", 0f);

        Settings.Invoke("FindNewTarget", 
            new Vector3(
                Model.GetFloat("GathererPositionX"),
                0.0f,
                Model.GetFloat("GathererPositionZ")                
                )
            );
    }
}
