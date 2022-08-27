using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Player;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int currencyReward = 20;
    [SerializeField] private int penalty = 5;

    private Bank _bank;
    private Sanity _playerSanity;
    
    
    void Start()
    {
        _bank = FindObjectOfType<Bank>();    
        _playerSanity = FindObjectOfType<Sanity>();    
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
        _playerSanity.TakeSanity(penalty);
    }
    

}
