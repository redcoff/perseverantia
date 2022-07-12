using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private float spawnTimer = 1f;
    [SerializeField] private int maxSpawns = 10;
    
    private float currentSpawned = 0f;

    private GameObject[] pool;

    private void Awake()
    {
        FillPool();
    }

    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    void FillPool()
    {
        pool = new GameObject[maxSpawns];

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
                return;
            }
        }
    }

    IEnumerator SpawnObject()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }

}
