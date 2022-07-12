using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform aim;
    [SerializeField] private ParticleSystem projectileParticles;
    [SerializeField] private float range = 15;

    void Update()
    {
        FindClosestTarget();
        Aim();
    }

    void FindClosestTarget()
    {
        var enemies = FindObjectsOfType<Enemy>();
        Transform closest = null;
        float maxDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (targetDistance < maxDistance)
            {
                closest = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closest;
    }

    void Aim()
    {
        var targetDistance = Vector3.Distance(transform.position, target.position);
        
        aim.LookAt(target);
        
        Attack(targetDistance < range);
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
