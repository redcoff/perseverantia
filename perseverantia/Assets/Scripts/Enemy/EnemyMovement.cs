using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f, 5f)] private float speed = 1f;

    private Enemy _enemy;
    private LevelController _levelController;
    private int _currentPathIndex;

    public int CurrentPathIndex => _currentPathIndex;
    
    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(Move());
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _levelController = FindObjectOfType<LevelController>();
        speed = (float)(speed + (_levelController.CurrentRound * (1 / 3.0)));
        Debug.Log(speed);
    }

    private void FindPath()
    {
        path.Clear();

        var waypointsObjects = GameObject.FindGameObjectsWithTag("Path").Select(obj => obj.GetComponent<Waypoint>()).ToList();
        path = waypointsObjects.OrderBy(point => point.Index).ToList();
    }

    void ReturnToStart()
    {
        transform.position = path[0].transform.position;
    }

    IEnumerator Move()
    {
        foreach (var point in path)
        {
            _currentPathIndex = point.Index;
            var startPosition = transform.position;
            var endPosition = point.transform.position;
            var travelPercent = 0f;

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }

        }
        
        EnemyPass();
    }

    void EnemyPass()
    {
        _enemy.HitPlayer();
        gameObject.SetActive(false);
    }

}
