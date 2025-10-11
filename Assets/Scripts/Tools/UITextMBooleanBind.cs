using AxGrid.Base;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UITextMBooleanBind : MonoBehaviourExt
{
    public string booleanToTrack = "";

    public string textTrue = "";
    public string textFalse = "";

    public bool defaultEnable = true;

    private TextMeshProUGUI text;

    [OnAwake]
    void TheAwake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    [OnStart]
    void TheStart()
    {
        Model.EventManager.AddAction($"On{booleanToTrack}Changed", OnBooleanChanged);

        OnBooleanChanged();

    }

    void OnBooleanChanged()
    {
        if (Model.GetBool(booleanToTrack, defaultEnable) == true) {
            text.text = textTrue;
        }
        else
        {
            text.text = textFalse;
        }
        
    }

    [OnDestroy]
    void DestroyThis()
    {
        Model.EventManager.RemoveAction($"On{booleanToTrack}Changed", OnBooleanChanged);
    }
}
