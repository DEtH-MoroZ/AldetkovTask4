using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_CS_Spawning")]
public class FSM_CS_Spawning : FSMState
{
    [Enter]
    private void EnterThis()
    {
        Debug.Log("FSM_CS_Spawning");
    }

    [Loop(0.2f)]
    void KeepSpawn()
    {
        Model.EventManager.Invoke("SpawnCoin");
        
    }
}
