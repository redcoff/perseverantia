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
    
    private void OnMouseDown()
    {
        if (isPlaceable)
        {
            var isCreated = tower.Create(new Vector3(transform.position.x, tower.transform.position.y, transform.position.z));
            isPlaceable = !isCreated;
        }
    }
}
