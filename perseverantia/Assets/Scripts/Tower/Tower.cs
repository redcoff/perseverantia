using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int cost = 70;
    [SerializeField] private AudioSource placeSource;
    
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
            placeSource.Play();
            bank.Withdraw(cost);
            return true;
        }

        return false;
    }
}
