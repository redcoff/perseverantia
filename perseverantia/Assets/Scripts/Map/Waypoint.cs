using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Tower tower;

    [SerializeField] private bool isPlaceable;
    public bool IsPlaceable => isPlaceable;

    public int Index;
    
    private LevelController _levelController;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
    }

    private void OnMouseDown()
    {
        if (_levelController.CurrentSelectedTower is not null)
        {
            _levelController.CurrentSelectedTower.HideRange();
            _levelController.CurrentSelectedTower = null;
        }
        
        if (isPlaceable)
        {
            var isCreated = tower.Create(new Vector3(transform.position.x, tower.transform.position.y, transform.position.z));
            isPlaceable = !isCreated;
        }
        
    }
}
