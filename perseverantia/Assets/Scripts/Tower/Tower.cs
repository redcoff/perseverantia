using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int cost = 70;
    [SerializeField] private AudioClip placeSound;
    
    private RangeCircle _rangeCircle;
    private LevelController _levelController;
    private bool isCircleDrawn = false;
    
    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
        _rangeCircle = GetComponentInChildren<RangeCircle>();
    }

    public bool Create(Vector3 position)
    {
        var bank = FindObjectOfType<Bank>();
        if (bank == null)
        {
            Debug.LogError("Bank instance is missing!");
            return false;
        }

        if (bank.CurrentBalance >= cost)
        {
            var newTower = Instantiate(gameObject, position, quaternion.identity);
            AudioSource.PlayClipAtPoint(placeSound, position);
            bank.Withdraw(cost);
            ShowRange(newTower.GetComponent<Tower>());
            return true;
        }

        return false;
    }

    public void HideRange()
    {
        _rangeCircle.DisableLine();
    }

    public void ShowRange(Tower tower)
    {
        _levelController ??= FindObjectOfType<LevelController>();
        _rangeCircle ??= GetComponentInChildren<RangeCircle>();

        Debug.Log(_levelController.CurrentSelectedTower);

        HidePreviousSelectedTower(tower);
        
        if (!isCircleDrawn)
        {
            _rangeCircle.Draw();
            isCircleDrawn = true;
        }
        
        _rangeCircle.EnableLine();
        _levelController.CurrentSelectedTower = tower;
        Debug.Log(_levelController.CurrentSelectedTower);
    }

    private void HidePreviousSelectedTower(Tower tower)
    {
        if (_levelController.CurrentSelectedTower != tower && _levelController.CurrentSelectedTower is not null)
        {
            _levelController.CurrentSelectedTower.HideRange();
        }
    }
    
    private void OnMouseDown()
    {
        Debug.Log(this);
        ShowRange(this);
    }
    
    
}
