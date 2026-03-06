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

        healthPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthPlayer>();
        healthText = transform.GetChild(0).GetComponent<TextMeshPro>();
        UpdateHealthText();
    }
    private void Start()
    {
        EventManager.PlayerTookDamage += UpdateHealthText;
        EventManager.EnemyDied += UpdateHealthText;
    }
    public void UpdateHealthText()
    {
        healthText.text = $"Health: {healthPlayer.Health}";
    }
}
