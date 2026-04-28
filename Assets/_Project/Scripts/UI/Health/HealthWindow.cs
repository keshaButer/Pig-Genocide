using VContainer;
using TMPro;
using UnityEngine;

public class HealthWindow : MonoBehaviour, IHealthWindow
{
    private HealthPlayer healthPlayer;
    private TextMeshPro healthText;
    [Inject] private IPlayerStateEvents _playerEvents;
    [Inject] private IEnemyEvents _enemyEvents;

    [Inject]
    public void Construct(IPlayerProvider playerProvider)
    {
        playerProvider.OnPlayerSpawned += OnPlayerSpawned;
        
        if (playerProvider.Player != null)
            OnPlayerSpawned(playerProvider.Player);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        _playerEvents.OnTookDamage += UpdateHealthText;
        _enemyEvents.OnEnemyDied += UpdateHealthText;

        healthPlayer = player.GetComponent<HealthPlayer>();
        healthText = transform.GetChild(0).GetComponent<TextMeshPro>();
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        healthText.text = $"Health: {healthPlayer.CurrentHealth}";
    }
}