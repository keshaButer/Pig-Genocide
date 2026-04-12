using UnityEngine;

public class DifficultyManager : IDifficultyManager
{
    public event System.Action<float> OnDifficultyChanged;

    private IInvokerFactory _invokerFactory;
    private IInvoker _invoker;
    private DifficultyConfig _config;
    private HealthPlayer _healthPlayer;
    private int _killCount;

    public DifficultyManager(DifficultyConfig config, IPlayerProvider playerProvider, IInvokerFactory invokerFactory)
    {
        _config = config;

        _invokerFactory = invokerFactory;

        EventManager.EnemyDied += () => _killCount++;

        playerProvider.OnPlayerSpawned += OnPlayerSpawned;
        if (playerProvider.Player != null)
        {
            OnPlayerSpawned(playerProvider.Player);
        }
    }

    private void OnPlayerSpawned(GameObject player)
    {
        _healthPlayer = player.GetComponent<HealthPlayer>();

        _invoker = _invokerFactory.StartRepeatInvoking(_config.ChangeRate, UpdateDifficulty);
    }

    private void UpdateDifficulty()
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
