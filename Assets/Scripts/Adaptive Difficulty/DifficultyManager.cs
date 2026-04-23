using System;
using UnityEngine;

public class DifficultyManager : IDifficultyManager, IDisposable
{
    public event Action<float> OnDifficultyChanged;

    private IInvokerFactory _invokerFactory;
    private IInvoker _invoker;
    private DifficultyConfig _config;
    private HealthPlayer _healthPlayer;
    private int _killCount;

    public DifficultyManager(DifficultyConfig config, IPlayerProvider playerProvider, IInvokerFactory invokerFactory, IEnemyEvents enemyEvents)
    {
        _config = config;

        _invokerFactory = invokerFactory;

        enemyEvents.OnEnemyDied += () => _killCount++;

        playerProvider.OnPlayerSpawned += OnPlayerSpawned;
        if (playerProvider.Player != null)
        {
            OnPlayerSpawned(playerProvider.Player);
        }
    }

    public void Dispose()
    {
        _invoker.Stop();            
    }

    private void OnPlayerSpawned(GameObject player)
    {
        _healthPlayer = player.GetComponent<HealthPlayer>();

        _invoker = _invokerFactory.StartRepeatInvoking(_config.ChangeRate, UpdateDifficulty);
    }

    public void UpdateDifficulty()
    {
        OnDifficultyChanged?.Invoke(GetPlayerSkill());
    }

    private float GetPlayerSkill()
    {
        if (_healthPlayer == null || _healthPlayer.CurrentHealth <= 0)
            return 0.5f;

        float healthRate = Mathf.Clamp(_healthPlayer.CurrentHealth / _healthPlayer.MaxHealth, 0.5f, 1.0f);
        float killRate = _killCount * _config.KillMeaning;
        
        float timeBonus = Time.timeSinceLevelLoad * _config.TimeMeaning;

        float skill = (timeBonus + killRate) * healthRate;

        return skill;
    }
}