using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager SingleTon;

    [HideInInspector] public List<Enemy> Enemies = new List<Enemy>();

    [SerializeField] private float _changeRate;
    [SerializeField] private float _killMeaning = 0.3f;
    [SerializeField] private float _reverseTimeMeaning = 60.0f;

    private HealthPlayer _healthPlayer;
    private int _kills;
    private float _timer;

    private void Awake()
    {
        if (SingleTon == null)
            SingleTon = this;
        else if (SingleTon != null)
            Destroy(this);

        PlayerSpawner.OnPlayerSpawned += Initialize;
        EventManager.EnemyDied += AddKill;
    }

    private void Initialize(GameObject player)
    {
        _healthPlayer = player.GetComponent<HealthPlayer>();

        InvokeRepeating(nameof(ChangeDifficulty), 0, _changeRate);
    }

    private void ChangeDifficulty()
    {
        Debug.Log($"Change difficulty with count enemies: {Enemies.Count}.");

        float playerSkill = GetPlayerSkill();
        foreach (Enemy enemy in Enemies)
        {
            enemy.ChangeDifficulty(playerSkill);
        }
    }

    private void AddKill() => _kills++;

    private float GetPlayerSkill()
    {
        if (_healthPlayer == null || _healthPlayer.CurrentHealth <= 0)
            return 0.5f;

        float healthRate = _healthPlayer.CurrentHealth / _healthPlayer.MaxHealth;
        float killRate = _kills * _killMeaning;
        
        float timeBonus = Time.timeSinceLevelLoad / _reverseTimeMeaning;

        float skill = (timeBonus + killRate) * healthRate;

        return skill;
    }

    public void Reset()
    {
        _kills = 0;
        _timer = 0;
        if (_healthPlayer != null)
            _healthPlayer = null;
        Enemies.Clear();
    }
}
