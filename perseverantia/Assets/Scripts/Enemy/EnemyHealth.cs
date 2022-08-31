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
        towersAoECanHit = new Dictionary<GameObject, bool>();

        maxHitPoints += _levelController.CurrentRound * 5;
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
    
    private Dictionary<GameObject, bool> towersAoECanHit;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Obelisk"))
        {
            Hit(obeliskDamageDealt);
        }
        if (other.CompareTag("Lamp"))
        {
            var canHit = true;
            if (towersAoECanHit.ContainsKey(other))
            {
                towersAoECanHit.TryGetValue(other, out canHit);
            }
            else
            {
                towersAoECanHit.TryAdd(other, true);
            }

            if (gameObject.activeSelf && canHit)
            {
                towersAoECanHit[other] = false;
                StartCoroutine(OnCooldown(other));
            }
        }
        if (other.CompareTag("SmallLamp"))
        {
            Hit(smallLampDamageDealt);
        }
    }

    IEnumerator OnCooldown(GameObject other)
    {
        Debug.Log(_currentHitPoints);
        yield return new WaitForSeconds(1);
        Hit(lampDamageDealt);
        towersAoECanHit[other] = true;
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
