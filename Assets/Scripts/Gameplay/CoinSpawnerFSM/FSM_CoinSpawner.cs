
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

	private Coin bumpCoin;

	private Vector3 bumpVector;

	private Transform bumpTransform;

	private readonly float Xarea = 10f;

	private readonly float Zarea = 10f;

	private readonly float height = 10f;

	private readonly float sideStep = 0.5f;

	private readonly float minTorqueMagnitude = 5f;

	private readonly float maxTorqueMagnitude = 15f;

	private CoinGrid coinGrid;

	private readonly float coinGridStep = 0.1f;

	private readonly float proximitySize = 2f;

	private Coin targetCoin;

	[OnAwake]
	private void awake()
	{
		coinList = new List<Coin>();
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
		for (int i = 0; i < coinList.Count; i++)
		{
			if (!coinList[i].gameObject.activeSelf)
			{
				bumpCoin = coinList[i];
				break;
			}
		}
		if (bumpCoin != null)
		{
			bumpVector = new Vector3(UnityEngine.Random.Range(transform.position.x - (Xarea - sideStep) / 2f, transform.position.x + (Xarea - sideStep) / 2f), height, UnityEngine.Random.Range(transform.position.z - (Zarea - sideStep) / 2f, transform.position.z + (Zarea - sideStep) / 2f));
			bumpCoin.transform.position = bumpVector;
			float num = UnityEngine.Random.Range(-1f, 1f);
			float num2 = UnityEngine.Random.Range(-1f, 1f);
			float num3 = UnityEngine.Random.Range(-1f, 1f);
			Vector3 val = new Vector3(num, num2, num3);
			Vector3 normalized = val.normalized;
			float num4 = UnityEngine.Random.Range(minTorqueMagnitude, maxTorqueMagnitude);
			bumpCoin.Spawn();
			bumpCoin.rb.AddTorque(normalized * num4, (ForceMode)1);
			coinGrid.Add(bumpCoin);
		}
		else
		{
			_fsm.Change("FSM_CS_Idle", false);
		}
		bumpCoin = null;
	}

	private void AddCoinToGrid(Coin theCoin)
	{
		coinGrid.Add(theCoin);
	}

	private void CoinDespawn(Coin theCoin)
	{
		coinGrid.Remove(theCoin);
		if (targetCoin != null && targetCoin == theCoin)
		{
			targetCoin = null;
			Model.EventManager.Invoke("TargetReached");
		}
	}

	private void FindNewTarget(Vector3 pos)
	{
		targetCoin = coinGrid.findCloses(pos);
		if (targetCoin != null)
		{
			Debug.Log(("tg coin id = " + targetCoin.GetInstanceID()));
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
		List<Coin> list = coinGrid.checkProximity(proximitySize / 2f, pos.x, pos.z);
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				CoinDespawn(list[i]);
			}
		}
	}
}
