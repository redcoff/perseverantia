using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class GameSettings
{
    public GameSetting[] settings;

    public GameSettings() { }

}

[Serializable]
public class GameSetting
{
    public string LevelName;
    public int[] EnemiesSetUpPerLevel;

    public GameSetting(string name, int[] enemies)
    {
        LevelName = name;
        EnemiesSetUpPerLevel = enemies;
    }
    
}
