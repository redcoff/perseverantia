using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using log4net.Core;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = System.Object;

public class EnemyLevelWindow : EditorWindow
{
    private bool _customSettingPerLevel = false;
    private Color _defaultLabelColor;
    private ObjectPool _enemyObjectPool;
    private int _totalRounds;
    private int _enemiesFirstRound;
    private bool _customizable = false;
    private string _currentLevelName;
    private int _currentLevelIndex;

    private LevelSettings _levelSettings;
    private GameSettings _gameSettings;

    private string _gameSettingsPath = "";

    private bool AreDataAlreadyLoaded = false;

    [SerializeField] List<int> Rounds = new();

    [MenuItem("Window/EnemyLevel")]
    public static void ShowWindow()
    {
        GetWindow<EnemyLevelWindow>("Enemy Level Design");
    }

    private void OnGUI()
    {
        _defaultLabelColor = GUI.contentColor;
        _currentLevelName = SceneManager.GetActiveScene().name;
        _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        _gameSettingsPath = Path.Combine(Application.dataPath, "settings.json");

        //GUILayout.BeginHorizontal();
        GUILayout.Label("Current level", EditorStyles.boldLabel);
        GUILayout.Box(_currentLevelName);
        //GUILayout.EndHorizontal();

        FindSettings();
        TotalRoundsSettings();
        EnemiesSettings();

        SaveSettings();
    }


    private void FindSettings()
    {
        var pool = FindObjectOfType<ObjectPool>();
        var levelSettings = FindObjectOfType<LevelSettings>();

        if (pool is null)
        {
            ShowErrorMessage("Enemy spawner is missing!");
        }

        if (levelSettings is null)
        {
            ShowErrorMessage("Level setter is missing!");
        }

        GUI.contentColor = _defaultLabelColor;
        _enemyObjectPool = pool;
        _levelSettings = levelSettings;

        if (!AreDataAlreadyLoaded)
        {
            LoadSavedGameSettings();
        }
    }
    

    private void LoadSavedGameSettings()
    {
        GameSettings settings = null;
        if (File.Exists(_gameSettingsPath))
        {
            var settingsContent = File.ReadAllText(_gameSettingsPath);
            settings = JsonUtility.FromJson<GameSettings>(settingsContent);
            Debug.Log("Game settings read");
        }

        if (settings is not null)
        {
            _gameSettings = settings;
            SetLocalSettings(settings);
        }
        else
        {
            _gameSettings = new GameSettings();
            _gameSettings.settings = new GameSetting[10];
        }

        AreDataAlreadyLoaded = true;
    }

    private void SetLocalSettings(GameSettings settings)
    {
        var settingsForCurrentLevel = new GameSetting(_currentLevelName, Rounds.ToArray());
        
        if (settings is null) return;
        var innerSettings = settings.settings.ToList();

        if (innerSettings.Any(s => s.LevelName.Equals(_currentLevelName)))
        {
            settingsForCurrentLevel =
                innerSettings.Find(obj => obj.LevelName.Equals(_currentLevelName));
        }

        if (settingsForCurrentLevel is not null)
        {
            var currentEnemyList = settingsForCurrentLevel.EnemiesSetUpPerLevel.ToList();
            _totalRounds = currentEnemyList.Count;
            _enemiesFirstRound = currentEnemyList[0];

            if (_levelSettings is not null)
            {
                _levelSettings.TotalRounds = _totalRounds;
                _levelSettings.EnemiesRounds = Rounds;
            }
        }
    }

    private void ShowErrorMessage(string message)
    {
        GUILayout.BeginHorizontal();
        GUI.contentColor = Color.red;
        GUILayout.Label(message, EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
    }

    private void TotalRoundsSettings()
    {
        _totalRounds = EditorGUILayout.IntField("Total rounds:", _totalRounds);
    }

    private void EnemiesSettings()
    {
        _enemiesFirstRound = EditorGUILayout.IntField("Total enemies /1st round:", _enemiesFirstRound);

        //_customizable = EditorGUILayout.Toggle("Customizable", _customizable); Za donate se zpristupni custom setovani levelu
        EditorGUI.BeginDisabledGroup(!_customizable);
        Rounds.Clear();
        for (var i = 0; i < _totalRounds; i++)
        {
            var value = _customizable ? 0 : CalculateTotalEnemies(_enemiesFirstRound, i);
            EditorGUILayout.IntField($"Level {i}", value);
            Rounds.Add(value);
        }

        EditorGUI.EndDisabledGroup();
    }

    private int CalculateTotalEnemies(int baseCount, int level)
    {
        return baseCount + (level * 5);
    }

    private void SaveSettings()
    {
        if (_levelSettings is null) return;
        if (_gameSettings is null) return;

        if (GUILayout.Button("Save"))
        {
            var innerSettings = _gameSettings.settings.ToList();
            
            if (innerSettings.Any(obj => obj is not null && obj.LevelName.Equals(_currentLevelName)))
            {
                var currentSettings = innerSettings.Find(obj => obj.LevelName.Equals(_currentLevelName));
                currentSettings.EnemiesSetUpPerLevel = Rounds.ToArray();
            }
            else
            {
                _gameSettings.settings[_currentLevelIndex] = new GameSetting(_currentLevelName, Rounds.ToArray());
            }

            var jsonContent = JsonUtility.ToJson(_gameSettings);
            File.WriteAllText(_gameSettingsPath, jsonContent);
            Debug.Log("Game Settings saved. ");
            AreDataAlreadyLoaded = false;
        }
    }
}