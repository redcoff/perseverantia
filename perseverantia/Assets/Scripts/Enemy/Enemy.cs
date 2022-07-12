using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int currencyReward = 20;
    [SerializeField] private int penalty = 5; //TODO: hit player HP

    private Bank _bank;
    
    void Start()
    {
        _bank = FindObjectOfType<Bank>();    
    }

    public void RewardCurrency()
    {
        if (_bank == null)
        {
            Debug.LogError("Bank instance is missing!");
            return;
        }
        
        _bank.Deposit(currencyReward);
    }

    public void HitPlayer()
    {
        //TODO: hit player HP
    }

}
