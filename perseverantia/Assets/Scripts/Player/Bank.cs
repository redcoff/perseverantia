using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] private int startingBalance = 150;

    private int currentBalance = 0;
    public int CurrentBalance => currentBalance;

    private void Awake()
    {
        currentBalance = startingBalance;
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);

        if (currentBalance < 0)
        {
            currentBalance = 0;
        }
    }

}
