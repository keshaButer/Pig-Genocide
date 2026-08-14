using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AudioSource))]

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private EnemyConfig _config;

    private IHealth _health;

    private void Awake()
    {
        _health = GetComponent<IHealth>();
        _health.OnDied += OnDied;
    }

    private void OnDied() => _source.PlayOneShot(_config.deathSound);

    private void OnDisable()
    {
        _health.OnDied -= OnDied;
    }
}