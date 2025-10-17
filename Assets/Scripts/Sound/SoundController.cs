using AxGrid.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviourExt
{
    public AudioClip[] coinDropCollection;
    public AudioClip[] coinPickupCollection;
    public AudioClip[] whooshCollection;
    public float coinDropVolume = 0.1f;

    [OnAwake]
    private void TheAwake()
    {
        Model.EventManager.AddAction<Vector3>("PlayCoinDrop", PlayCoinDrop);
        Model.EventManager.AddAction<Vector3>("PlayCoinPickUp", PlayCoinPickUp);
        Model.EventManager.AddAction<Vector3>("PlayCoinWhoosh", PlayCoinWhoosh);
    }


    private void PlayCoinDrop(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(coinDropCollection[Random.Range(0, coinDropCollection.Length - 1)], pos, coinDropVolume);
    }        

    private void PlayCoinPickUp(Vector3 pos)
    {        
        AudioSource.PlayClipAtPoint(coinPickupCollection[Random.Range(0, coinPickupCollection.Length - 1)], pos, coinDropVolume);
    }

    private void PlayCoinWhoosh(Vector3 pos) {
        AudioSource.PlayClipAtPoint(whooshCollection[Random.Range(0, whooshCollection.Length - 1)], pos, coinDropVolume);
    }


    [OnDestroy]
    private void TheDestroy()
    {
        Model.EventManager.RemoveAction<Vector3>("PlayCoinDrop", PlayCoinDrop);
        Model.EventManager.RemoveAction<Vector3>("PlayCoinPickUp", PlayCoinPickUp);
        Model.EventManager.RemoveAction<Vector3>("PlayCoinWhoosh", PlayCoinWhoosh);
    }
}
