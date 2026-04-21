using VContainer;
using TMPro;
using UnityEngine;

public class HealthWindow : MonoBehaviour, IHealthWindow
{
    private HealthPlayer healthPlayer;
    private TextMeshPro healthText;

    [Inject]
    public void Construct(IPlayerProvider playerProvider)
    {
        playerProvider.OnPlayerSpawned += OnPlayerSpawned;
        
        if (playerProvider.Player != null)
            OnPlayerSpawned(playerProvider.Player);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        EventManager.PlayerTookDamage += UpdateHealthText;
        EventManager.EnemyDied += UpdateHealthText;

        healthPlayer = player.GetComponent<HealthPlayer>();
        healthText = transform.GetChild(0).GetComponent<TextMeshPro>();
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        healthText.text = $"Health: {healthPlayer.CurrentHealth}";
    }
}