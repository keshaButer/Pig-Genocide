using VContainer;
using System;
using UnityEngine;

public class HealthPlayer : MonoBehaviour, IDamagable, IHealth
{
    [Range(0, 15)]
    [SerializeField] private int _maxHealth = 10;
    public int MaxHealth => _maxHealth;

    [SerializeField] bool _isImmortalMode;

    public event Action<int> OnHealthChanged;
    public event Action OnDied;
    private int _health;
    private bool _wasDeath;

    [Inject] private IPlayerStateEvents _playerStateEvents;

    public int CurrentHealth
    {
        get => _health;

        private set
        {
            if (_isImmortalMode) return;

            int newHealth = Math.Clamp(value, 0, MaxHealth);

            _health = newHealth;
            OnHealthChanged?.Invoke(newHealth);

            if (value <= 0 && !_wasDeath)
                Die();
        }
    }

    private void Start() => CurrentHealth = MaxHealth;

    private void Die()
    {
        _wasDeath = true;
        OnDied?.Invoke();
    }

    public void AddHP(int hp) => CurrentHealth += hp;

    public void ApplyDamage(int damage)
    {
        if (_wasDeath) return;

        CurrentHealth -= damage;

        _playerStateEvents.NotifyTookDamage();
    }
}
