using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int cost = 70;
    
    public bool Create(Vector3 position)
    {
        var bank = FindObjectOfType<Bank>();
        if (bank == null)
        {
            Debug.LogError("Bank instance is missing!");
            return false;
        }

        if (bank.CurrentBalance >= cost)
        {
            Instantiate(gameObject, position, quaternion.identity);
            bank.Withdraw(cost);
            return true;
        }

        return false;
    }
}
