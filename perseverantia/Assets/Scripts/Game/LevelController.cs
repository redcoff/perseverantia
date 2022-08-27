using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int CurrentRound { get; private set;}
    public LevelState CurrentState { get; private set; }
    
    private LevelSettings _levelSettings;
    private ObjectPool _objectPool;
    private GameUIController _gameUIController;

    private void Awake()
    {
        _levelSettings = FindObjectOfType<LevelSettings>();
        _objectPool = FindObjectOfType<ObjectPool>();
        _gameUIController = FindObjectOfType<GameUIController>();
    }

    public void DespawnEnemy()
    {
        _objectPool.AddDespawned();
    }

    public void FinishRound()
    {
        CurrentRound++;
        RunBreak();
    }

    void Start()
    {
        RunBreak();
    }

    private void RunBreak()
    {
        _gameUIController.UpdateRound(CurrentRound+1, _levelSettings.TotalRounds);
        _gameUIController.PrepareBreak();
        CurrentState = LevelState.Break;
    }

    public void RunRound()
    {
        if (CurrentState is LevelState.EnemiesRound
            || CurrentRound > _levelSettings.TotalRounds) return;

        CurrentState = LevelState.EnemiesRound;
        _objectPool.Run();
    }
}

public enum LevelState
{
    EnemiesRound,
    Break
}
