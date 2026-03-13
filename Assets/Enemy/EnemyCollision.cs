using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private EnemyConfig _config;
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnCollisionEnter(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<HealthPlayer>(out HealthPlayer player))
        {
            player.ApplyDamage(_config.collisionDamage);
            _enemy.HandleCollision(other);
        }
    }
}
