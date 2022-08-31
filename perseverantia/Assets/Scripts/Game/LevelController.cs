using System;
using System.Collections;
using System.Collections.Generic;
using AlpaSunFade;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public int CurrentRound { get; private set;}
    public LevelState CurrentState { get; private set; }
    
    private LevelSettings _levelSettings;
    private ObjectPool _objectPool;
    private GameUIController _gameUIController;
    [SerializeField] private TransitionPanel _transitionPanel;
    
    private Bank _happiness;

    public Tower CurrentSelectedTower;

    private void Awake()
    {
        _levelSettings = FindObjectOfType<LevelSettings>();
        _objectPool = FindObjectOfType<ObjectPool>();
        _gameUIController = FindObjectOfType<GameUIController>();
        _happiness = FindObjectOfType<Bank>();
        
        SavePlayerScoreLevelStarted();
        CurrentSelectedTower = null;
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

    public void GameOver()
    {
        SetCurrentPlayerScore(false);
        _transitionPanel.StartTransition(true, 0, 2);
        StartCoroutine(Waiter(true));
    }

    private void EndLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var totalScenesCount = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex == totalScenesCount - 2)
        {
            SetCurrentPlayerScore(true);
            _transitionPanel.StartTransition(true, 0, 2);
            StartCoroutine(Waiter());
        }
        else
        {
            SetCurrentPlayerScore(false);
            _transitionPanel.StartTransition(true, 0, 2);
            StartCoroutine(Waiter());
        }
    }

    IEnumerator Waiter(bool isGameOver = false)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(isGameOver ? SceneManager.sceneCountInBuildSettings - 1 : SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private void SavePlayerScoreLevelStarted()
    {
        var totalRounds = PlayerPrefs.GetInt("TotalRounds", 0);
        var happinessLeft = PlayerPrefs.GetInt("HappinessLeft", 0);
        
        PlayerPrefs.SetInt("PreviousTotalRounds", totalRounds);
        PlayerPrefs.SetInt("PreviousHappinessLeft", happinessLeft);
    }
    
    private void SetCurrentPlayerScore(bool isWin)
    {
        var totalRounds = PlayerPrefs.GetInt("TotalRounds", 0);
        var happinessLeft = PlayerPrefs.GetInt("HappinessLeft", 0);

        PlayerPrefs.SetInt("TotalRounds", totalRounds + CurrentRound);
        PlayerPrefs.SetInt("HappinessLeft", happinessLeft + _happiness.CurrentBalance);
        PlayerPrefs.SetInt("CurrentLevelIndex", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("IsWin", isWin ? 1 : 0);
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
