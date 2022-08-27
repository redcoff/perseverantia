using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class GameSettings
{
    [SerializeField]
    public Dictionary<string, List<int>> EnemiesSetUpPerLevel = new Dictionary<string, List<int>>();

    public GameSettings() { }

}
