using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private int maxHealth;
    public int CurrentHealth { get; private set; }
    public event Action OnDied;
    public event Action<int> OnHealthChanged;

    private void Awake() => CurrentHealth = maxHealth;

    public void ApplyDamage(int damage)
    {
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(CurrentHealth);
        
        if (CurrentHealth <= 0)
            Die();
    }

    private void Die()
    {
        OnDied?.Invoke();
        Destroy(this);
    }
}
