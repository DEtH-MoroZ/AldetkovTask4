using AxGrid;
using AxGrid.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBind : MonoBehaviourExtBind
{
    
    
    public string gameObjectName = "";

    public string enableField = "";
        
    public bool defaultEnable = true;

    [OnAwake]
    void TheAwake()
    {   
        if (string.IsNullOrEmpty(gameObjectName))
            gameObjectName = gameObject.name;

        enableField = enableField == "" ? $"GO{gameObjectName}Enable" : enableField;

    }

    [OnStart]
    void TheStart()
    {
        Model.EventManager.AddAction($"On{enableField}Changed", OnGoEnable);
        
        OnGoEnable();

    }

    void OnGoEnable()
    {
        if (gameObject.activeSelf != Model.GetBool(enableField, defaultEnable))
        {
            gameObject.SetActive(Model.GetBool(enableField, defaultEnable));
        }
    }

    [OnDestroy]
    void DestroyThis()
    {
        Model.EventManager.RemoveAction($"On{enableField}Changed", OnGoEnable);
    }
}
