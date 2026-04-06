using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyConfig Config;

    protected Rigidbody2D RigidBody { get; private set; }
    protected Health HealthComponent { get; private set; }

    protected bool IsDead { get; private set; }

    protected virtual void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        HealthComponent = GetComponent<Health>();

        HealthComponent.OnDied += HandleDeath;
    }
    protected virtual void Start()
    {
        DifficultyManager.SingleTon.Enemies.Add(this);
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

        DifficultyManager.SingleTon.Enemies.Remove(this);
    }

    public abstract void ChangeDifficulty(float playerSkill);
}
