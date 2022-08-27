using System;
using System.Collections;
using System.Collections.Generic;
using AlpaSunFade;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    [SerializeField] public int TotalRounds;
    [SerializeField] private ObjectPool _objectPool;
    public List<int> EnemiesRounds;

    [SerializeField] private int _tower1Price;
    [SerializeField] private int _tower2Price;
    [SerializeField] private int _tower3Price;
    
    [SerializeField] private int _startingSanity;
    [SerializeField] private int _startingHappiness;

    public int Tower1Price => _tower1Price;
    public int Tower2Price => _tower2Price;
    public int Tower3Price => _tower3Price;
    public int StartingSanity => _startingSanity;
    public int StartingHappiness => _startingHappiness;
    
}
