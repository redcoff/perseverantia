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

    private LevelSettings _levelSettings;
    private GameSettings _gameSettings;

    private string _gameSettingsPath = Path.Combine(Application.persistentDataPath, "settings.json");

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

        LoadSavedGameSettings();
    }

    private void LoadSavedGameSettings()
    {
        var settings = new GameSettings();
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
        }
    }

    private void SetLocalSettings(GameSettings settings)
    {
        if (settings?.EnemiesSetUpPerLevel is null) return;

        settings.EnemiesSetUpPerLevel.TryGetValue(_currentLevelName, out var levelRounds);
        if (levelRounds is not null)
        {
            _totalRounds = levelRounds.Count;
            _enemiesFirstRound = levelRounds[0];
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
            if (_gameSettings.EnemiesSetUpPerLevel.ContainsKey(_currentLevelName))
            {
                _gameSettings.EnemiesSetUpPerLevel[_currentLevelName] = Rounds;
            }
            else
            {
                _gameSettings.EnemiesSetUpPerLevel.TryAdd(_currentLevelName, Rounds);
            }

            _gameSettings.EnemiesSetUpPerLevel.TryGetValue(_currentLevelName, out var levelRounds);
            if (levelRounds is not null)
            {
                _levelSettings.TotalRounds = levelRounds.Count;
                _levelSettings.EnemiesRounds = levelRounds;
            }

            Debug.Log(_gameSettings.EnemiesSetUpPerLevel.Count);

            var jsonContent = JsonUtility.ToJson(_gameSettings);
            Debug.Log(jsonContent);
            File.WriteAllText(_gameSettingsPath, jsonContent);
            Debug.Log("Game Settings saved. ");
        }
    }
}