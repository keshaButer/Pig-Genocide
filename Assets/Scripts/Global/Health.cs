using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private int maxHealth;

    public int CurrentHealth { get; private set; }
    public event Action OnDied;
    public event Action<int> OnHealthChanged;

    private bool _isDead;

    private void Awake() => CurrentHealth = maxHealth;

    public void Initialize(int health)
    {
        maxHealth = health;
        CurrentHealth = health;
        _isDead = false;
    }

    /// <summary>
    /// Applies damage to this entity. Triggers OnHealthChanged and possibly OnDied.
    /// </summary>
    public void ApplyDamage(int damage)
    {
        if (damage < 0) 
        {
            Debug.LogWarning("Negative damage ignored");
            return;
        }

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        OnHealthChanged?.Invoke(CurrentHealth);
        
        if (CurrentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (_isDead) 
            return;

        _isDead = true;

        OnDied?.Invoke();
        Destroy(this);
    }
}
