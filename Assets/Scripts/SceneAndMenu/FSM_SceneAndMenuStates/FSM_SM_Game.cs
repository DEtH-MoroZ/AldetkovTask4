using AxGrid;
using AxGrid.Base;
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
            Model.EventManager.AddAction("OnGameToMenuClick", OnGameToMenuClick);

            if (SceneManager.GetActiveScene().buildIndex != 2)
            {
                SceneManager.LoadScene(2);
            }
            Debug.Log("[FSM_SM] Game scene loaded.");
        }
        private void OnGameToMenuClick()
        {
            Parent.Change("FSM_SM_MainMenu");
        }

        [OnDestroy]
        private void TheDestroy()
        {
            Model.EventManager.RemoveAction("OnGameToMenuClick", OnGameToMenuClick);
        }
    }
}
