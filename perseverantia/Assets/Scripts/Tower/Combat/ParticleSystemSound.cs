using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemSound : MonoBehaviour
{
    private ParticleSystem _parentParticleSystem;

    public AudioClip AttackSound;

    private IDictionary<uint, ParticleSystem.Particle> _trackedParticles =
        new Dictionary<uint, ParticleSystem.Particle>();

    private AudioSource _singleSound;

    void Start()
    {
        _parentParticleSystem = GetComponent<ParticleSystem>();
        if (_parentParticleSystem == null)
            Debug.LogError("Missing ParticleSystem!", this);
    }

    void Update()
    {
        var liveParticles = new ParticleSystem.Particle[_parentParticleSystem.particleCount];
        _parentParticleSystem.GetParticles(liveParticles);

        var particleDelta = GetParticleDelta(liveParticles);


        foreach (var particleAdded in particleDelta.Added)
        {
            if (CompareTag("Lamp") || CompareTag("Obelisk"))
            {
                if (_singleSound is null)
                {
                    PlaySound(true);
                }
                else if (!_singleSound.isPlaying)
                {
                    Destroy(_singleSound);
                    _singleSound = null;
                }
            }
            else
            {
                PlaySound(false);
            }
        }

        if (CompareTag("Lamp") || CompareTag("Obelisk"))
        {
            if ( !_parentParticleSystem.isPlaying && _singleSound is not null)
            {
                _singleSound.loop = false;
                _singleSound.DOFade(0f, 1);
            }
        }
        else
        {
            foreach (var particleRemoved in particleDelta.Removed)
            {
                RemovePlayedSound();
            }
        }
    }

    private void PlaySound(bool loop)
    {
        if (_singleSound is not null) Destroy(_singleSound);

        var sound = gameObject.AddComponent<AudioSource>();
        sound.clip = AttackSound;
        sound.pitch = UnityEngine.Random.Range(.7f, 1.2f);
        sound.loop = loop;
        sound.Play();
        
        if (CompareTag("Lamp") || CompareTag("Obelisk")) _singleSound = sound;
    }

    private void RemovePlayedSound()
    {
        var sounds = GetComponents<AudioSource>();
        foreach (var sound in sounds)
        {
            if (!sound.isPlaying)
            {
                Destroy(sound);
            }
        }
    }

    private ParticleDelta GetParticleDelta(ParticleSystem.Particle[] liveParticles)
    {
        var deltaResult = new ParticleDelta();

        foreach (var activeParticle in liveParticles)
        {
            ParticleSystem.Particle foundParticle;
            if (_trackedParticles.TryGetValue(activeParticle.randomSeed, out foundParticle))
            {
                _trackedParticles[activeParticle.randomSeed] = activeParticle;
            }
            else
            {
                deltaResult.Added.Add(activeParticle);
                _trackedParticles.Add(activeParticle.randomSeed, activeParticle);
            }
        }

        var updatedParticleAsDictionary = liveParticles.ToDictionary(x => x.randomSeed, x => x);
        var dictionaryKeysAsList = _trackedParticles.Keys.ToList();

        foreach (var dictionaryKey in dictionaryKeysAsList)
        {
            if (updatedParticleAsDictionary.ContainsKey(dictionaryKey) == false)
            {
                deltaResult.Removed.Add(_trackedParticles[dictionaryKey]);
                _trackedParticles.Remove(dictionaryKey);
            }
        }

        return deltaResult;
    }

    private class ParticleDelta
    {
        public IList<ParticleSystem.Particle> Added { get; set; } = new List<ParticleSystem.Particle>();
        public IList<ParticleSystem.Particle> Removed { get; set; } = new List<ParticleSystem.Particle>();
    }
}