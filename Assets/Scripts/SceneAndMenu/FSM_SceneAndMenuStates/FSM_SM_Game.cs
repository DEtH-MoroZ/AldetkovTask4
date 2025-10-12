using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using System;
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
            Model.EventManager.AddAction("OnGameToMenuFadeOut", OnGameToMenuFadeOut);


            if (SceneManager.GetActiveScene().buildIndex != 2)
            {
                SceneManager.LoadScene(2);
            }
            Debug.Log("[FSM_SM] Game scene loaded.");
        }
        private void OnGameToMenuClick()
        {
            Model.EventManager.Invoke("FadeOut", "OnGameToMenuFadeOut");            
        }

        private void OnGameToMenuFadeOut()
        {
            Parent.Change("FSM_SM_MainMenu");
        }

        [One(1f)]
        private void TheOne()
        {
            Model.EventManager.Invoke("FadeIn", "");
        }
        [OnDestroy]
        private void TheDestroy()
        {
            Model.EventManager.RemoveAction("OnGameToMenuClick", OnGameToMenuClick);
            Model.EventManager.RemoveAction("OnGameToMenuFadeOut", OnGameToMenuFadeOut);
        }
    }
}
