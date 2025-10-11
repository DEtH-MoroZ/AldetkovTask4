
using System;
using System.Collections.Generic;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using UnityEngine;

public class FSM_CoinSpawner : MonoBehaviourExt
{
	private readonly int StartCoinCount = 100;
	private readonly int MaxCoinCount = 1000;

	private List<Coin> coinList;
	private FSM _fsm;
	public Transform[] CoinPrefab;
    private CoinGrid coinGrid;

    private Coin bumpCoin;
	private Vector3 bumpVector;
	private Transform bumpTransform;

	private readonly float Xarea = 10f;
	private readonly float Zarea = 10f;
	private readonly float height = 10f;
	private readonly float sideStep = 0.5f; //distance between spawn area and its edge. nessesarry to keep objects in place
    private readonly float coinGridStep = 0.2f;
    private readonly float proximityRadius = 0.5f;

    private readonly float minTorqueMagnitude = 5f;
	private readonly float maxTorqueMagnitude = 15f;	

	private Coin targetCoin;

	[OnAwake]
	private void awake()
	{		
		PrepairCoins();
		coinGrid = new CoinGrid(Xarea, coinGridStep);

		Model.EventManager.AddAction<Vector3>("FindNewTarget", FindNewTarget);
		Model.EventManager.AddAction<Vector3>("CheckProxymityAndDespawn", CheckProxymityAndDespawn);
		Model.EventManager.AddAction<Coin>("AddCoinToGrid", AddCoinToGrid);

		Model.Set("InitialCoinCount", StartCoinCount);

		Model.EventManager.AddAction("SpawnCoin", SpawnCoin);
		
		_fsm = new FSM();
		_fsm.Add(new FSM_CS_Initial() );
		_fsm.Add(new FSM_CS_Idle() );
		_fsm.Add(new FSM_CS_Spawning());
		_fsm.Start("FSM_CS_Initial");
	}

	private void PrepairCoins()
	{
        coinList = new List<Coin>();

        for (int i = 0; i < MaxCoinCount; i++)
		{
			bumpTransform = Instantiate<Transform>(CoinPrefab[i % CoinPrefab.Length]);
			bumpTransform.SetParent(this.transform);
			coinList.Add(bumpTransform.GetComponent<Coin>());
		}
	}

	[OnUpdate]
	public void UpdateFsm()
	{
		_fsm.Update(Time.deltaTime);
	}


	private void SpawnCoin()
	{
        bumpCoin = null;
        for (int i = 0; i < coinList.Count; i++) //looking for inactive, despawned coin.
		{
			if (!coinList[i].gameObject.activeSelf && !coinList[i].registered)
			{
				bumpCoin = coinList[i];
				break;
			}
		}
		if (bumpCoin != null) //if found, then spawn. new random position, random torque for rotation
		{
			bumpVector = new Vector3(
				UnityEngine.Random.Range(transform.position.x - (Xarea - sideStep) / 2f, transform.position.x + (Xarea - sideStep) / 2f), 
				height, 
				UnityEngine.Random.Range(transform.position.z - (Zarea - sideStep) / 2f, transform.position.z + (Zarea - sideStep) / 2f));
			bumpCoin.transform.position = bumpVector;

			bumpVector = new Vector3(
				UnityEngine.Random.Range(-1f, 1f),
				UnityEngine.Random.Range(-1f, 1f),
				UnityEngine.Random.Range(-1f, 1f)
				).normalized;
			bumpCoin.Spawn();
			
			bumpCoin.rb.AddTorque(bumpVector * UnityEngine.Random.Range(minTorqueMagnitude, maxTorqueMagnitude), (ForceMode)1);
			//coinGrid.Add(bumpCoin); 
		}
		else
		{
			_fsm.Change("FSM_CS_Idle");
		}		
	}

	private void AddCoinToGrid(Coin theCoin) //adding to grid only when collectable is on grownd
    {
		coinGrid.Add(theCoin);
	}

	private void FindNewTarget(Vector3 pos)
	{
		targetCoin = coinGrid.findCloses(pos);
		if (targetCoin != null)
		{
			Model.Set("TargetX", targetCoin.transform.position.x);
			Model.Set("TargetZ", targetCoin.transform.position.z);
			Model.EventManager.Invoke("TargetFound");
		}
		else
		{
			Model.EventManager.Invoke("TargetNotFound");
		}
	}

	private void CheckProxymityAndDespawn(Vector3 pos)
	{
		List<Coin> list = coinGrid.CheckProximity(proximityRadius, pos.x, pos.z);
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
                CoinDespawnAndCheckForTarget(list[i]);
			}
		}
	}
    private void CoinDespawnAndCheckForTarget(Coin theCoin)
    {
        coinGrid.Remove(theCoin);
        if (targetCoin == theCoin)
        {
            targetCoin = null;
            Model.EventManager.Invoke("TargetReached");
        }
		if (_fsm.CurrentStateName == "FSM_CS_Idle")
		_fsm.Change("FSM_CS_Spawning");
    }

    [OnDestroy]
    private void TheDestroy()
    {
        Model.EventManager.RemoveAction<Vector3>("FindNewTarget", FindNewTarget);
        Model.EventManager.RemoveAction<Vector3>("CheckProxymityAndDespawn", CheckProxymityAndDespawn);
        Model.EventManager.RemoveAction<Coin>("AddCoinToGrid", AddCoinToGrid);

        Model.EventManager.RemoveAction("SpawnCoin", SpawnCoin);
    }
}
