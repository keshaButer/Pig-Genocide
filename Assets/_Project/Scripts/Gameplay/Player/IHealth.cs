using System;

public interface IHealth
{
    event Action<int> OnHealthChanged;
    int CurrentHealth { get; }
    int MaxHealth { get; }
}