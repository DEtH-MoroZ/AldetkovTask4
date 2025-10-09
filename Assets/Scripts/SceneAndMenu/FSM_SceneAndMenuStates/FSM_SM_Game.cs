using AxGrid;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManager_FSM
{
    [State("FSM_SM_Game")]
    public class FSM_SM_Game : FSMState
    {

        [Enter]
        private void EnterThis()
        {
            if (SceneManager.GetActiveScene().buildIndex != 2)
            {
                SceneManager.LoadScene(2);
            }
            Debug.Log("[FSM_SM] Game scene loaded.");
        }

    }

}
