using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif



namespace SceneManager_FSM
{
    [State("FSM_SM_MainMenu")]
    public class FSM_SM_MainMenu : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Model.EventManager.AddAction("OnMainMenuStartGameClick", OnMainMenuStartGameClick);
            Model.EventManager.AddAction("OnMainMenuExitGameClick", OnMainMenuExitGameClick);

            Model.EventManager.AddAction("OnFadeOutStartGame", OnFadeOutStartGame);
            Model.EventManager.AddAction("OnFadeOutExitGame", OnFadeOutExitGame);

            Model.EventManager.Invoke("FadeIn", "");

            if (SceneManager.GetActiveScene().buildIndex != 1)
            {
                SceneManager.LoadScene(1);
            }            
            Debug.Log("[FSM_SM] MainMenu scene loaded.");

            
        }

        private void OnMainMenuStartGameClick()
        {
            Model.EventManager.Invoke("FadeOut", "OnFadeOutStartGame");

        }

        private void OnFadeOutStartGame()
        {
            Parent.Change("FSM_SM_Game");

        }

        private void OnMainMenuExitGameClick()
        {
            Model.EventManager.Invoke("FadeOut", "OnFadeOutExitGame");
        }

        private void OnFadeOutExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }

        [One(1f)]
        private void TheOne()
        {
            Model.EventManager.Invoke("FadeIn", "");
        }

        [OnDestroy]
        private void TheDestroy()
        {
            Model.EventManager.RemoveAction("OnMainMenuStartGameClick", OnMainMenuStartGameClick);
            Model.EventManager.RemoveAction("OnMainMenuExitGameClick", OnMainMenuExitGameClick);
            Model.EventManager.RemoveAction("OnFadeOutStartGame", OnFadeOutStartGame);
            Model.EventManager.RemoveAction("OnFadeOutExitGame", OnFadeOutExitGame);
        }                
    }
}
