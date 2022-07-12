using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class CoordLabel : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private Color blockedColor = Color.gray;
    
    private TextMeshPro _label;
    private Vector2Int _coordinates = new Vector2Int();
    private Waypoint _waypoint;

    private void Awake()
    {
        _label = GetComponent<TextMeshPro>();
        _label.enabled = false;
        
        _waypoint = GetComponentInParent<Waypoint>();
        DisplayCoordinates();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
        }

        ColorCoordinates();
        ToggleLabels();
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _label.enabled = !_label.IsActive();
        }
    }

    void DisplayCoordinates()
    {
        _coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        _coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);
        
        _label.text = $"[{_coordinates.x.ToString()}, {_coordinates.y.ToString()}]";
    }

    void UpdateObjectName()
    {
        transform.parent.name = _coordinates.ToString();
    }

    void ColorCoordinates()
    {
        _label.color = _waypoint.IsPlaceable ? defaultColor : blockedColor;
    }
}
