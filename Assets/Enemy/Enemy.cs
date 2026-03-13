using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyConfig _config;

    protected Rigidbody2D RigidBody { get; private set; }
    protected Health HealthComponent { get; private set; }

    protected bool IsDead { get; private set; }

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        HealthComponent = GetComponent<Health>();

        HealthComponent.OnDied += HandleDeath;
    }

    private void HandleDeath()
    {
        if (IsDead) return;
        IsDead = true;

        RigidBody.constraints = RigidbodyConstraints2D.None;
        RigidBody.linearVelocity = Vector2.zero;
        RigidBody.mass = _config.massOnDeath;

        DisableComponents();
        EventManager.OnEnemyDied();

        Destroy(gameObject, _config.delayToDestroy);
    }

    protected virtual void DisableComponents() { }
    
    private void OnDestroy()
    {
        if (HealthComponent != null)
        {
            HealthComponent.OnDied -= HandleDeath;
        }
    }
}
