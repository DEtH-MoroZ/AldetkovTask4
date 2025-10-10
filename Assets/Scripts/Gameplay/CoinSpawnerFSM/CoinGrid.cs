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
        if (gridSideLength < 0 )
        {
            Debug.LogError("Grid Side Length must be more than zero.");
        }
        _gridStep = gridStep;
        if (gridStep < 0)
        {
            Debug.LogError("Grid Step must be more than zero.");
        }

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

        int coinPosX = WorldToGridIndices(coin.transform.position.x);
        int coinPosZ = WorldToGridIndices(coin.transform.position.z);

        
        for (int a = CoinGridList[coinPosX][coinPosZ].Count - 1; a >= 0; a--) //make sure to avoid duplicates
        {
            if (CoinGridList[coinPosX][coinPosZ][a].gameObject.GetInstanceID() == coin.gameObject.GetInstanceID())
            {
                Debug.Log("duplicate "+ coin.GetInstanceID());
                return;
            }
        }
        
        CoinGridList[coinPosX][coinPosZ].Add(coin);
    }

    public void Remove(Coin coin)
    {
        if (CoinGridList == null)
        {
            Debug.LogWarning("[CoinGrid] No coin grid presented!");
            return;
        }
        if (coin.registered == false) return;

        int coinPosX = WorldToGridIndices(coin.transform.position.x);
        int coinPosZ = WorldToGridIndices(coin.transform.position.z);

        //Debug.Log("remove " + coin.GetInstanceID() + " " + coin.transform.position + " " + coinPosX + " " + coinPosZ);

        CoinGridList[coinPosX][coinPosZ].Remove(coin);
        coin.Despawn();        

        /*
        for (int a = CoinGridList[coinPosX][coinPosZ].Count - 1; a >= 0; a--) {

            //Debug.Log("removed " + CoinGridList[coinPosX][coinPosZ][a].GetInstanceID() + " " + CoinGridList[coinPosX][coinPosZ][a].transform.position);

            if (CoinGridList[coinPosX][coinPosZ][a].gameObject.GetInstanceID() == coin.gameObject.GetInstanceID()) {

                
                CoinGridList[coinPosX][coinPosZ][a].Despawn();
                CoinGridList[coinPosX][coinPosZ].RemoveAt(a);                                
                
                //Debug.Log(coin.GetInstanceID());
            }
        }
        */

    }
    public Coin findCloses(Vector3 pos)
    {

        int posX = WorldToGridIndices(pos.x);
        int posZ = WorldToGridIndices(pos.z);
     
        //Debug.Log("Gatherer position = " + pos.x + " " + pos.z + " " + posX + " " + posZ);

        List<Coin> found = CoinGridSpiralFinder.FindNearestCoins(CoinGridList, posX, posZ);
        if (found != null)
        {            //if (found[0].registered == false) Debug.Log("found unregistered");
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


    public List<Coin> CheckProximity(float proximityHalfSize, float centerX, float centerZ)
    {
        
        if (proximityHalfSize <= 0)
        {
            Debug.LogError("Proximity Half Size should be greater than zero.");
            return null;
        }


        int indexHalfSize =Mathf.FloorToInt( proximityHalfSize / _gridStep);
        int indexCenterX = WorldToGridIndices(centerX);
        int indexCenterZ = WorldToGridIndices(centerZ);
                
        List<Coin> coinsAround = new List<Coin>();

        for (int a = indexCenterX - indexHalfSize; a < indexCenterX + indexHalfSize; a++)

        {
            
            for (int b = indexCenterZ - indexHalfSize; b < indexCenterZ + indexHalfSize; b++)
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

    private int WorldToGridIndices (float value)
    {
        return Mathf.FloorToInt((value + (_gridSideLength / 2f)) / _gridStep);         
    }
}
