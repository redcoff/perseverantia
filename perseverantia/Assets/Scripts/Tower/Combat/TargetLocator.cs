using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform aim;
    [SerializeField] private ParticleSystem projectileParticles;
    [SerializeField] private float range = 15;
    [SerializeField] private bool isAOE = false;

    public float Range => range;
    

    private bool onCooldown = false;
    
    
    void Update()
    {
        FindTarget();
        Aim();
    }
    

    void FindTarget()
    {
        var enemies = FindObjectsOfType<Enemy>();
        
        Transform closest = null;
        var maxDistance = range;
        var minPathIndex = Mathf.NegativeInfinity;

        foreach (var enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            var targetPathIndex = enemy.GetComponent<EnemyMovement>().CurrentPathIndex;

            if (targetDistance < maxDistance && targetPathIndex > minPathIndex)
            {
                closest = enemy.transform;
                minPathIndex = targetPathIndex;
            }
        }

        target = closest;
    }

    void Aim()
    {
        if (target is null)
        {
           StopAttack();
           return;
        }
        var targetDistance = Vector3.Distance(transform.position, target.position);

        if (targetDistance > range)
        {
            StopAttack();
            return;
        }

        if (!isAOE)
        {
            aim.DOLookAt(target.position, 1f);
        }
        
        Attack();
    }

    void Attack()
    {
        if (projectileParticles.isPlaying) return;

        projectileParticles.Play();
    }

    void StopAttack()
    {
        if (!projectileParticles.isPlaying) return;
        
        projectileParticles.Stop();
    }
    
}
