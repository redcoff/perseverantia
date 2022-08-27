using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackSoundPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem ParticleSystem;
    [SerializeField] private AudioSource AttackSound;
    
    void OnParticleTrigger()
    {
        Debug.Log("fcking trigger");
        AttackSound.Play();
    }
}
