using TMPro;
using UnityEngine;

public class HealthWindow : MonoBehaviour
{
    public static HealthWindow SingleTon;
    private HealthPlayer healthPlayer;
    private TextMeshPro healthText;
    private void Awake()
    {
        if (SingleTon == null)
            SingleTon = this;
        else Destroy(this);

        PlayerSpawner.OnPlayerSpawned += Initialize;
    }

    private void OnDisable() => PlayerSpawner.OnPlayerSpawned -= Initialize;

    public void Initialize(GameObject player)
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
