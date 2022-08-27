using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    private int currentBalance = 0;
    private GameUIController _gameUIController; 
    private LevelSettings _levelSettings;
        
    public int CurrentBalance => currentBalance;

    private void Awake()
    {
        _levelSettings = FindObjectOfType<LevelSettings>();
        currentBalance = _levelSettings.StartingHappiness;
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
