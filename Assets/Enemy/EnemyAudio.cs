using UnityEngine;

[RequireComponent(typeof(Health))]

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private EnemyConfig _config;

    private void Awake()
    {
        GetComponent<Health>().OnDied += () => _source.PlayOneShot(_config.deathSound);
    }
}
