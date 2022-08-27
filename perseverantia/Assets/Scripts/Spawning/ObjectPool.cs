using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private float spawnTimer = 1f;

    public int MaximumSpawns = 10;

    private int currentSpawned = 0;
    private int currentDespawned = 0;
    private GameObject[] pool;
    
    private LevelController _levelController;
    private LevelSettings _levelSettings;

    //UnityEvent EndEvent;

    private void Awake()
    {
         _levelController = FindObjectOfType<LevelController>();
         _levelSettings = FindObjectOfType<LevelSettings>();
    }

    void Start()
    {
        /*if (EndEvent == null)
            EndEvent = new UnityEvent();

        EndEvent.AddListener(Finish);*/
    }

    public void AddDespawned()
    {
        Debug.Log("Despawned: " + currentDespawned);
        currentDespawned++;

        if (currentDespawned != 0 && currentSpawned != 0 && currentDespawned == currentSpawned)
        {
            Finish();
        }
    }

    public void Run()
    {
        ResetRound();
        FillPool();
        StartCoroutine(SpawnObject());
    }

    void FillPool()
    {
        MaximumSpawns = _levelSettings.EnemiesRounds[_levelController.CurrentRound];
        
        Debug.Log($"Current num of enemies: {MaximumSpawns.ToString()}");
        
        pool = new GameObject[MaximumSpawns];

        for (var i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(objectPrefab, transform);
            pool[i].SetActive(false);
        }

    }

    void EnableObjectInPool()
    {
        for (var i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                currentSpawned++;
                return;
            }
        }
    }

    IEnumerator SpawnObject()
    {
        while (currentSpawned < MaximumSpawns)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }

    private void ResetRound()
    {
        currentDespawned = 0;
        currentSpawned = 0;
    }

    private void Finish()
    {
        Debug.Log("Round finished.");
        _levelController.FinishRound();
    }

}
