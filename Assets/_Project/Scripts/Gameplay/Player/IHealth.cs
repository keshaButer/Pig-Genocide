using System;

public interface IHealth
{
    event Action<int> OnHealthChanged;
    event Action OnDied;
    int CurrentHealth { get; }
    int MaxHealth { get; }
}