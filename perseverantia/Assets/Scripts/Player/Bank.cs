using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] private int startingBalance = 150;

    private int currentBalance = 0;
    private GameUIController _gameUIController; 
        
    public int CurrentBalance => currentBalance;

    private void Awake()
    {
        currentBalance = startingBalance;
        _gameUIController = FindObjectOfType<GameUIController>(); 
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        _gameUIController.UpdateHappiness(CurrentBalance);
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);

        if (currentBalance < 0)
        {
            currentBalance = 0;
        }
        
        _gameUIController.UpdateHappiness(CurrentBalance);
    }

}
