using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using AlpaSunFade;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private TransitionPanel _transitionPanel;

    private UIDocument _doc;

    private Label _titleLabel;
    private Label _subtitleLabel;
    
    private Label _totalRoundsLabel;
    private Label _happinessLeftLabel;
    private Label _totalScoreLabel;

    private Button _resetLevelButton;
    private Button _mainMenuButton;

    private const string WinTitle = "you have overcome";
    private const string WinSubtitle = "your bad dreams.";
    
    private const string LoseTitle = "Game over";
    private const string LoseSubtitle = "and your mind was filled by endless darkness.";
    

    private void Awake()
    {
        _transitionPanel.StartTransition(false, 1, 1);
        _doc = GetComponent<UIDocument>();

        _titleLabel = _doc.rootVisualElement.Q<Label>("GameOverLabel");
        _subtitleLabel = _doc.rootVisualElement.Q<Label>("FinalTextLabel");
        
        var isWin = PlayerPrefs.GetInt("IsWin", 1);
        _titleLabel.text = isWin == 1 ? WinTitle : LoseTitle;
        _subtitleLabel.text = isWin == 1 ? WinSubtitle : LoseSubtitle;

        _totalRoundsLabel = _doc.rootVisualElement.Q<Label>("RoundsValue");
        _happinessLeftLabel = _doc.rootVisualElement.Q<Label>("HappinessLeftValue");
        _totalScoreLabel = _doc.rootVisualElement.Q<Label>("TotalScoreValue");

        _resetLevelButton = _doc.rootVisualElement.Q<Button>("ResetLevelButton");
        _mainMenuButton = _doc.rootVisualElement.Q<Button>("MainMenuButton");

        _resetLevelButton.clicked += ResetLevelClicked;
        _mainMenuButton.clicked += MainMenuClicked;

        var totalRounds = PlayerPrefs.GetInt("TotalRounds", 0);
        var happinessLeft = PlayerPrefs.GetInt("HappinessLeft", 0);
        _totalRoundsLabel.text = totalRounds.ToString();
        _happinessLeftLabel.text = happinessLeft.ToString();
        _totalScoreLabel.text = (totalRounds * happinessLeft).ToString();
    }


    private void ResetLevelClicked()
    {
        var currentTotalRounds = PlayerPrefs.GetInt("PreviousTotalRounds");
        var currentHappinessLeft = PlayerPrefs.GetInt("PreviousHappinessLeft");
        var currentLevel = PlayerPrefs.GetInt("CurrentLevelIndex");
        
        PlayerPrefs.SetInt("TotalRounds", currentTotalRounds);
        PlayerPrefs.SetInt("HappinessLeft", currentHappinessLeft);
        
        _transitionPanel.StartTransition(true, 0, 2);
        StartCoroutine(LoadScene(currentLevel));
    }


    private void MainMenuClicked()
    {
        _transitionPanel.StartTransition(true, 0, 2);
        StartCoroutine(LoadScene(0));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneIndex);
    }
}
