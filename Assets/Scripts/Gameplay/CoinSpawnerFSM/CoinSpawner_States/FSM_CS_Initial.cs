using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_CS_Initial")]
public class FSM_CS_Initial : FSMState
{

    [Enter]
    private void EnterThis()
    {
        Debug.Log("FSM_CS_Initial");

        int InitialCoinCount = Model.Get<int>("InitialCoinCount");

        for (int a = 0; a < InitialCoinCount ; a++)
        {
            Model.EventManager.Invoke("SpawnCoin");
        }

        Parent.Change("FSM_CS_Spawning");
    }

}
