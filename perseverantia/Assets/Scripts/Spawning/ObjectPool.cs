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

    private float currentSpawned = 0f;
    private GameObject[] pool;

    UnityEvent EndEvent;

    void Start()
    {
        if (EndEvent == null)
            EndEvent = new UnityEvent();

        EndEvent.AddListener(Finish);
    }

    public void Run()
    {
        FillPool();
        StartCoroutine(SpawnObject());
    }

    void FillPool()
    {
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

    private void Finish()
    {
        
    }

}
