using AxGrid.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviourExt
{
    public ParticleSystem CoinPickUpVFX;

    [OnAwake]
    private void TheAwake()
    {
        Model.EventManager.AddAction<Vector3>("PlayCoinPickUpVFX", PlayCoinPickUpVFX);
    }

    private void PlayCoinPickUpVFX(Vector3 pos)
    {
        CoinPickUpVFX.transform.position = pos;
        CoinPickUpVFX.Play();
    }


    [OnDestroy]
    private void TheDestroy()
    {
        Model.EventManager.RemoveAction<Vector3>("PlayCoinPickUpVFX", PlayCoinPickUpVFX);
    }
}
