using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a pool of particle effects for reuse
/// Avoids creating new objects, reuses stopped effects
/// </summary>
public class ParticleEffectPool
{
    private readonly List<ParticleSystem> _availableEffects = new();
    private readonly List<ParticleSystem> _activeEffects = new();
    private readonly ParticleSystem _prefab;
    private readonly Transform _parent;

    public ParticleEffectPool(ParticleSystem prefab, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
    }

    /// <summary>
    /// Get an effect from the pool or create a new one
    /// </summary>
    public ParticleSystem GetEffect()
    {
        ParticleSystem effect;

        if (_availableEffects.Count > 0)
        {
            effect = _availableEffects[0];
            _availableEffects.RemoveAt(0);
        }
        else
        {
            effect = Object.Instantiate(_prefab, _parent);
        }

        _activeEffects.Add(effect);
        return effect;
    }

    /// <summary>
    /// Play an effect at the specified position
    /// </summary>
    public void PlayEffectAt(Vector3 position)
    {
        var effect = GetEffect();
        effect.transform.position = position;
        effect.Play();
    }

    /// <summary>
    /// Return an effect to the pool (called when the effect finishes)
    /// </summary>
    public void ReturnEffect(ParticleSystem effect)
    {
        if (_activeEffects.Contains(effect))
        {
            _activeEffects.Remove(effect);
            _availableEffects.Add(effect);
        }
    }

    /// <summary>
    /// Update pool state (move completed effects back to pool)
    /// </summary>
    public void UpdatePool()
    {
        var toReturn = new List<ParticleSystem>();
        
        foreach (var effect in _activeEffects)
        {
            if (effect.isStopped)
            {
                toReturn.Add(effect);
            }
        }

        foreach (var effect in toReturn)
        {
            ReturnEffect(effect);
        }
    }
}
