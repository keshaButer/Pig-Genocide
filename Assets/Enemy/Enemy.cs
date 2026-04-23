using VContainer;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public abstract class Enemy : MonoBehaviour
{
    [Inject] private readonly IDifficultyManager _difficultyManager;
    [Inject] private readonly IEnemyEvents _enemyEvents;

    public EnemyConfig Config;

    protected Rigidbody2D RigidBody { get; private set; }
    protected Health HealthComponent { get; private set; }

    protected bool IsDead { get; private set; }

    protected virtual void OnEnable()
    {
        _difficultyManager.OnDifficultyChanged += OnDifficultyChanged;
    }
    
    protected virtual void OnDisable()
    {
        _difficultyManager.OnDifficultyChanged -= OnDifficultyChanged;
        if (HealthComponent != null)
            HealthComponent.OnDied -= HandleDeath;
    }

    protected virtual void Awake()
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
        RigidBody.mass = Config.massOnDeath;

        OnDeath();
        _enemyEvents.NotifyEnemyDied();

        Destroy(gameObject, Config.delayToDestroy);
    }

    protected virtual void OnDeath() { }

    public virtual void HandleCollision(Collision2D other) { }
    
    protected virtual void OnDestroy()
    {
        if (HealthComponent != null)
        {
            HealthComponent.OnDied -= HandleDeath;
        }
    }

    public abstract void OnDifficultyChanged(float playerSkill);
}