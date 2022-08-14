using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int CurrentRound { get; }
    
    private LevelSettings _levelSettings;
    private ObjectPool _objectPool;
    
    private void Awake()
    {
        _levelSettings = FindObjectOfType<LevelSettings>();
        _objectPool = FindObjectOfType<ObjectPool>();
    }

    void Start()
    {
        RunRound();
    }

    private void RunRound()
    {
        Debug.Log($"Run {CurrentRound} round");
        _objectPool.Run();
    }
    
    

}
