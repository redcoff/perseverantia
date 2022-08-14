using System;
using System.Collections.Generic;
using log4net.Core;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = System.Object;

public class EnemyLevelWindow : EditorWindow
{
    //co potrebuju
    //aktualni scenu - to si setnu do editoru, co je moje aktualni scena - jestli je to validni level?
    // -- potrebuju tam mit ten object pool. pokud chybi, editor zahlasi, ze chybi. Mozna by se skrz nej mohlo aj vytvorit.
    // muzu urcit pocet kol
    // muzu urcit pocet enemies per kolo - spis nejaky base a pak by se dopocitalo adekvatni pocet podle kola
    // jako podle obtiznosti - v dalsim kole by jich bylo o neco vic

    private bool _customSettingPerLevel = false;
    private Color _defaultLabelColor;
    private ObjectPool _enemyObjectPool;
    private int _totalRounds;
    private int _enemiesFirstRound;
    private bool _customizable = false;

    private LevelSettings _levelSettings;
    
    [SerializeField] List<int> Rounds = new();

    [MenuItem("Window/EnemyLevel")]
    public static void ShowWindow()
    {
        GetWindow<EnemyLevelWindow>("Enemy Level Design");
    }

    private void OnGUI()
    {
        _defaultLabelColor = GUI.contentColor;
        
        //GUILayout.BeginHorizontal();
        GUILayout.Label("Current level", EditorStyles.boldLabel);
        GUILayout.Box(SceneManager.GetActiveScene().name);
        //GUILayout.EndHorizontal();

        EnemyObjectPoolSettings();
        TotalRoundsSettings();
        EnemiesSettings();

        SaveSettings();
    }

    private void EnemyObjectPoolSettings()
    {
        var pool =  FindObjectOfType<ObjectPool>();
        var levelSettings = FindObjectOfType<LevelSettings>();

        if (pool is null)
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.red;
            ShowMissingObjectLabel("Enemy spawner");
            GUILayout.EndHorizontal();
        }

        if (levelSettings is null)
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.red;
            ShowMissingObjectLabel("Level setter");
            GUILayout.EndHorizontal();
        }

        GUI.contentColor = _defaultLabelColor;
        _enemyObjectPool = pool;
        _levelSettings = levelSettings;
    }

    private void TotalRoundsSettings()
    {
        _totalRounds =  EditorGUILayout.IntField("Total rounds:", _totalRounds);
    }

    private void EnemiesSettings()
    {
        _enemiesFirstRound = EditorGUILayout.IntField("Total enemies /1st round:", _enemiesFirstRound);

        _customizable = EditorGUILayout.Toggle("Customizable", _customizable);
        EditorGUI.BeginDisabledGroup(!_customizable);
        for (var i = 1; i < _totalRounds; i++)
        {
            var value = _customizable ? 0 : CalculateTotalEnemies(_enemiesFirstRound, i);
            EditorGUILayout.IntField($"Level {i}", value);
            Rounds.Add(value);
        }
        EditorGUI.EndDisabledGroup();

    }

    private void ShowMissingObjectLabel(string name)
    {
        GUILayout.BeginHorizontal();
        GUI.contentColor = Color.red;
        GUILayout.Label($"{name} is missing!");
        GUILayout.EndHorizontal();
    }

    private int CalculateTotalEnemies(int baseCount, int level)
    {
        return baseCount + (level * 5);
    }

    private void SaveSettings()
    {
        if (_levelSettings is not null)
        {
            _levelSettings.TotalRounds = _totalRounds;
            _levelSettings.EnemiesRounds = Rounds;
        }
    }



}
