using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 5;
    private int _currentHitPoints = 0;
    private Enemy _enemy;

    void OnEnable()
    {
        _currentHitPoints = maxHitPoints;
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Hit();
    }

    private void Hit()
    {
        Debug.Log("au");
        _currentHitPoints--;
        if (_currentHitPoints <= 0)
        {
            _enemy.RewardCurrency();
            gameObject.SetActive(false);
        }
    }
}
