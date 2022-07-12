using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f, 5f)] private float speed = 1f;

    private Enemy _enemy;
    
    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(Move());
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void FindPath()
    {
        path.Clear();

        var waypoints = GameObject.FindGameObjectsWithTag("Path");

        foreach (var point in waypoints)
        {
            path.Add(point.GetComponent<Waypoint>());
        }
    }

    void ReturnToStart()
    {
        transform.position = path[0].transform.position;
    }

    IEnumerator Move()
    {
        foreach (var point in path)
        {
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
