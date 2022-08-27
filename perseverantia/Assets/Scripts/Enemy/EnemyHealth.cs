using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 20;
    
    [SerializeField] private int obeliskDamageDealt = 3;
    [SerializeField] private int lampDamageDealt = 10;
    [SerializeField] private int smallLampDamageDealt = 5;
    
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

    //TODO: dictionary s objektem towery a value jestli muze hitnout
    private bool canHit = true;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Obelisk"))
        {
            Hit(obeliskDamageDealt);
        }
        if (other.CompareTag("Lamp"))
        {
            if (gameObject.activeSelf && canHit)
            {
                canHit = false;
                StartCoroutine(OnCooldown());
            }
        }
        if (other.CompareTag("SmallLamp"))
        {
            Hit(smallLampDamageDealt);
        }
    }

    IEnumerator OnCooldown()
    {
        Debug.Log(_currentHitPoints);
        yield return new WaitForSeconds(1);
        Hit(lampDamageDealt);
        canHit = true;
    }

    private void Hit(int damage)
    {
        _currentHitPoints -= damage;
        if (_currentHitPoints <= 0)
        {
            _enemy.RewardCurrency();
            gameObject.SetActive(false);
            
        }
    }
}
