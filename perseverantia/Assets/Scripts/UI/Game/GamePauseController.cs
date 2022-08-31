using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GamePauseController : MonoBehaviour
{
    private UIDocument _doc;
    private Button _mainMenuButton;
    private Button _exitGameButton;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _mainMenuButton = _doc.rootVisualElement.Q<Button>("MainMenuButton");
        _exitGameButton = _doc.rootVisualElement.Q<Button>("EndGameButton");
        
        _mainMenuButton.clicked += MainMenuClick;
        _exitGameButton.clicked += ExitGameClick;
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void MainMenuClick()
    {
        SceneManager.LoadScene(0);
    }

    private void ExitGameClick()
    {
        Application.Quit();
    }
}
