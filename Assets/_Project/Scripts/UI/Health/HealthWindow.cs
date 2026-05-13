using VContainer;
using TMPro;
using UnityEngine;

public class HealthWindow : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;

    private IHealth _playerHealth;

    [Inject]
    public void Construct(IPlayerProvider playerProvider)
    {
        playerProvider.OnPlayerSpawned += OnPlayerSpawned;

        if (playerProvider.Player != null)
            OnPlayerSpawned(playerProvider.Player);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        _playerHealth = player.GetComponent<IHealth>();
        _playerHealth.OnHealthChanged += UpdateHealthText;
    }

    private void UpdateHealthText(int health)
    {
        _text.text = $"Health: {health}";
    }

    private void OnDisable()
    {
        if (_playerHealth != null)
            _playerHealth.OnHealthChanged -= UpdateHealthText;
    }
}