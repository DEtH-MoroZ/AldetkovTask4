using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManager_FSM
{
    [State("FSM_SM_MainMenu")]
    public class FSM_SM_MainMenu : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Model.EventManager.AddAction("OnMainMenuStartGameClick", OnMainMenuStartGameClick);
            
            if (SceneManager.GetActiveScene().buildIndex != 1)
            {
                SceneManager.LoadScene(1);
            }            
            Debug.Log("[FSM_SM] MainMenu scene loaded.");
        }

        private void OnMainMenuStartGameClick()
        {
            Parent.Change("FSM_SM_Game");
        }
        /*
        [Exit]
        private void ExitThis()
        {
            onDestroy();
        }
        */
        [OnDestroy]
        private void TheDestroy()
        {
            Model.EventManager.RemoveAction("OnMainMenuStartGameClick", OnMainMenuStartGameClick);
        }
    }
}
