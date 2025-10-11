using AxGrid.FSM;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_CS_Idle")]
public class FSM_CS_Idle : FSMState
{
    [Enter]
    private void OnEnter () {
        Debug.Log("FSM_CS_Idle");
    }
}
