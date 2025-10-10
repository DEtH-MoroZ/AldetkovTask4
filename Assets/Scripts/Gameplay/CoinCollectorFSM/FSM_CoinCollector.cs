using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class FSM_CoinCollector : MonoBehaviourExt {

    private FSM _fsm;



    [OnAwake]
    private void TheAwake()
    {
        Model.EventManager.AddAction("OnStartButtonClick", OnStartButtonClick);
        Model.EventManager.AddAction("OnStopButtonClick", OnStopButtonClick);
        Model.EventManager.AddAction("OnRunButtonClick", OnRunButtonClick);

        Model.Set("RunMode", false);

        Model.EventManager.AddAction("TargetReached", OnTargetReached);

    }

    [OnStart]
    private void TheStart()
    {
        _fsm = new FSM();

        _fsm.Add(new FSM_CC_Initial());
        _fsm.Add(new FSM_CC_Idle());
        _fsm.Add(new FSM_CC_FindTarget());        
        _fsm.Add(new FSM_CC_Move());
        
        _fsm.Start("FSM_CC_Initial");
    }

    private void OnStartButtonClick()
    {
        _fsm.Change("FSM_CC_FindTarget");
    }
    private void OnStopButtonClick()
    {
        _fsm.Change("FSM_CC_Idle");
    }
    private void OnRunButtonClick()
    {
        Model.Set("RunMode", !Model.Get<bool>("RunMode"));
    }

    private void OnTargetReached()
    {
        Debug.Log("[FSM_CC] Target reached.");
        if (_fsm.CurrentStateName != "FSM_CC_Idle")
        {            
            _fsm.Change("FSM_CC_FindTarget");
        }
    }

    [OnDestroy]
    public void onDestroy()
    {
        Model.EventManager.RemoveAction("OnStartButtonClick", OnStartButtonClick);
        Model.EventManager.RemoveAction("OnStopButtonClick", OnStopButtonClick);
        Model.EventManager.RemoveAction("OnRunButtonClick", OnRunButtonClick);

        Model.EventManager.RemoveAction("TargetReached", OnTargetReached);
    }

    [OnUpdate]
    public void UpdateFsm()
    {
        _fsm.Update(Time.deltaTime);
    }
}
