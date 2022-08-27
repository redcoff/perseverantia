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

    private LevelController _levelController;

    private bool _spawned = false;
    private bool _instanced = false;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
        _spawned = false;
    }

    void OnEnable()
    {
        if (!_instanced)
        {
            _instanced = true;
            return;
        } 
        
        _currentHitPoints = maxHitPoints;
        _spawned = true;
    }

    private void OnDisable()
    {
        if (!_spawned) return;
        
        _spawned = false;
        _levelController.DespawnEnemy();
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
