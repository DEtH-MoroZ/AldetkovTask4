using AxGrid;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManager_FSM
{
    [State("FSM_SM_FirstLoadingScreen")]
    public class FSM_SM_FirstLoadingScreen : FSMState
    {
        [Enter]
        private void EnterThis()
        {

            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                Debug.Log("[FSM_SM] Already loaded");
                SceneManager.LoadScene(0);
            }
            Debug.Log("[FSM_SM] FirstLoadingScreen loaded.");            
        }

        [One(1f)]
        private void GoToMainMenu()
        {
            Parent.Change("FSM_SM_MainMenu");
        }
    }
}
