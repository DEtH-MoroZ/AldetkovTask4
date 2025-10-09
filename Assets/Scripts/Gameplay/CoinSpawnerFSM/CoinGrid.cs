using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CoinGrid //class to hold information about coins. nessesarry for optimizations, that makes it all possible
{
    float _gridSideLength = 10f;
    float _gridStep = 0.1f; //size of on cell

    int _gridSideCellCount;

    private List<Coin>[][] CoinGridList;
    public CoinGrid (float gridSideLength, float gridStep)
    {
        _gridSideLength = gridSideLength;
        _gridStep = gridStep;

        _gridSideCellCount = Mathf.FloorToInt(_gridSideLength / _gridStep);

        CoinGridList = new List<Coin>[_gridSideCellCount][];

        for (int a = 0; a < _gridSideCellCount; a++)
        {
            CoinGridList[a] = new List<Coin>[_gridSideCellCount];
            for (int b = 0; b < _gridSideCellCount; b++)
            {
                CoinGridList[a][b] = new List<Coin>();
            }
        }
    }




    public void Add(Coin coin)
    {
        if (CoinGridList == null)
        {
            Debug.LogWarning("[CoinGrid] No coin grid presented!");
            return;
        }

        //if (coin.enabled) {return; }

        int coinPosX = Mathf.FloorToInt(SnapToGrid(coin.transform.position.x + _gridSideLength/2f, _gridStep) / _gridStep);
        int coinPosZ = Mathf.FloorToInt(SnapToGrid(coin.transform.position.z + _gridSideLength/2f, _gridStep) / _gridStep);

        //Debug.Log(coinPosX + " " + coin.transform.position.x + " " +  coinPosZ + " " + coin.transform.position.z + coin.GetInstanceID());

        if (CoinGridList[coinPosX][coinPosZ].Contains(coin))
        {
            Debug.Log("contains");
            return;
        }

        CoinGridList[coinPosX][coinPosZ].Add(coin);
        coin.Spawn();
    }
    public void Remove(Coin coin)
    {
        if (CoinGridList == null)
        {
            Debug.LogWarning("[CoinGrid] No coin grid presented!");
            return;
        }

        int coinPosX = Mathf.FloorToInt(SnapToGrid(coin.transform.position.x + _gridSideLength / 2f, _gridStep) / _gridStep);
        int coinPosZ = Mathf.FloorToInt(SnapToGrid(coin.transform.position.z + _gridSideLength / 2f, _gridStep) / _gridStep);
        CoinGridList[coinPosX][coinPosZ].Remove(coin);
        coin.Despawn();
    }

    public Coin findCloses(Vector3 pos)
    {
        int posX = Mathf.FloorToInt(SnapToGrid(pos.x + _gridSideLength / 2f, _gridStep) / _gridStep);
        int posZ = Mathf.FloorToInt(SnapToGrid(pos.z + _gridSideLength / 2f, _gridStep) / _gridStep);
        
        List<Coin> found = CoinGridSpiralFinder.FindNearestCoins(CoinGridList, posX, posZ);
        if (found != null)
        {
            return found[0];
        }
        else
        {
            return null;
        }

        /*
        for (int a = 0; a < CoinGridList.Length; a++)
        {
            for (int b = 0; b < CoinGridList[a].Length; b++)
            {
                for (int c = 0; c < CoinGridList[a][b].Count; c++)
                {
                    if (CoinGridList[a][b][c] != null) {
                        return CoinGridList[a][b][c];
                    }
                }
            }
        }

        return null;*/

    }


    public List<Coin> checkProximity(float halfSize, float centerX, float centerZ)
    {
        
        if (halfSize <= 0)
        {
            Debug.LogError("halfSize should be greater than zero.");
            return null;
        }

        int groundedHalfSize =Mathf.FloorToInt(SnapToGrid(halfSize , _gridStep)/_gridStep);
        int groundedCenterX = Mathf.FloorToInt(SnapToGrid(centerX + _gridSideLength / 2f, _gridStep) / _gridStep);
        int groundedCenterZ = Mathf.FloorToInt(SnapToGrid(centerZ + _gridSideLength / 2f, _gridStep) / _gridStep);
                
        List<Coin> coinsAround = new List<Coin>();

        for (int a = groundedCenterX - groundedHalfSize; a < groundedCenterX + groundedHalfSize; a++)

        {
            
            for (int b = groundedCenterZ - groundedHalfSize; b < groundedCenterZ + groundedHalfSize; b++)
            {
                if (a < 0 ||  b < 0 || a >= CoinGridList.Length || b >= CoinGridList[a].Length)
                {
                    continue;
                }

                for (int c = 0; c < CoinGridList[a][b].Count; b++)
                {
                    if (CoinGridList[a][b][c])
                    {
                        coinsAround.Add( CoinGridList[a][b][c] );
                    }
                }
            }
        }
        
        return coinsAround;
    }

    private float SnapToGrid(float value, float gridSize)
    {
        return Mathf.Round(value / gridSize) * gridSize;
    }
}
