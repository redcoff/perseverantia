using System;
using System.Collections;
using System.Collections.Generic;
using AlpaSunFade;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    [SerializeField] public int TotalRounds;
    public List<int> EnemiesRounds;

    private ObjectPool _enemyObjectPool;

    [SerializeField] private int _tower1Price;
    [SerializeField] private int _tower2Price;
    [SerializeField] private int _tower3Price;
    
    [SerializeField] private TransitionPanel _transitionPanel;

    public int Tower1Price => _tower1Price;
    public int Tower2Price => _tower2Price;
    public int Tower3Price => _tower3Price;

    private void Awake()
    {
        _enemyObjectPool =  FindObjectOfType<ObjectPool>();
        _transitionPanel.StartTransition(false, 0, 3);
    }
}
