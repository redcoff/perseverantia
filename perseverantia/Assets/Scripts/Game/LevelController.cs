using System;
using System.Collections;
using System.Collections.Generic;
using AlpaSunFade;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEditor.SearchService.Scene;

public class LevelController : MonoBehaviour
{
    public int CurrentRound { get; private set;}
    public LevelState CurrentState { get; private set; }
    
    private LevelSettings _levelSettings;
    private ObjectPool _objectPool;
    private GameUIController _gameUIController;
    [SerializeField] private TransitionPanel _transitionPanel;

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

        if (CurrentRound == _levelSettings.TotalRounds)
        {
            EndLevel();
            return;
        }
        
        RunBreak();
    }

    private void EndLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var totalScenesCount = SceneManager.sceneCountInBuildSettings;
        
        Debug.Log(currentSceneIndex);
        Debug.Log(totalScenesCount);
        
        if (currentSceneIndex == totalScenesCount)
        {
            //konec hry
            Debug.Log("Endgame");
        }
        else
        {
            _transitionPanel.StartTransition(true, 0, 2);
            StartCoroutine(Waiter());
        }
    }
    
    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Start()
    {
        _transitionPanel.StartTransition(false, 1, 1);
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
        Debug.Log(CurrentState);
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
