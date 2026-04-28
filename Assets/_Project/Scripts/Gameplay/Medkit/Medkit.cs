using VContainer;
using UnityEngine;

public class Medkit : MonoBehaviour, IInteractableObject
{
    [SerializeField] private int _healthAmount;

    [Inject] private IPlayerProvider _playerProvider;

    private HealthPlayer _healthPlayer;

    private void Start()
    {
        _playerProvider.OnPlayerSpawned += OnPlayerSpawned;

        if (_playerProvider.Player != null)
        {
            OnPlayerSpawned(_playerProvider.Player);
        }
    }

    private void OnPlayerSpawned(GameObject player)
    {
        _healthPlayer = player.GetComponent<HealthPlayer>();
    }

    public void Interact()
    {
        if (_healthPlayer == null)
        {
            Debug.Log("Health Player is null.");
            return;
        }

        _healthPlayer.AddHP(_healthAmount);

        Destroy(gameObject);
    }
}