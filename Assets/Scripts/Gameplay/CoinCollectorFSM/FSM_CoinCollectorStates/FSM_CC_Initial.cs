using AxGrid;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_CC_Initial")]
public class FSM_CC_Initial : FSMState
{
    [Enter]
    private void EnterThis()
    {        
        Parent.Change("FSM_CC_Idle");
    }
}
