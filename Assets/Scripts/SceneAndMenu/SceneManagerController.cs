using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using ExampleFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

namespace SceneManager_FSM
{

    public class SceneManagerController : MonoBehaviourExtBind
    {
        private FSM _fsm;
        
        [OnAwake]
        private void TheAwake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        [OnStart]
        private void TheStart()
        {
            Model.EventManager.AddAction("OnMainMenuExitGameClick", OnMainMenuExitGameClick);

            _fsm = new FSM();

            _fsm.Add(new FSM_SM_FirstLoadingScreen());
            _fsm.Add(new FSM_SM_Game());
            _fsm.Add(new FSM_SM_MainMenu());

            _fsm.Start("FSM_SM_FirstLoadingScreen");            
            
            Log.Debug("[SceneManagerController] Ready");
        }

        [OnUpdate] //on fixedupdate atleast??
        private void UpdateFsm()
        {
            _fsm.Update(Time.deltaTime);  
        }

        private void OnMainMenuExitGameClick()
        {
            Application.Quit();
        }

        [OnDestroy]
        private void TheDestroy()
        {
            Model.EventManager.RemoveAction("OnMainMenuExitGameClick", OnMainMenuExitGameClick);
        }        
    }
}

