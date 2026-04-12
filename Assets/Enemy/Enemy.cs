using VContainer;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public abstract class Enemy : MonoBehaviour
{
    [Inject] private IDifficultyManager _difficultyManager;

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
        EventManager.OnEnemyDied();

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

    protected abstract void OnDifficultyChanged(float playerSkill);
}
