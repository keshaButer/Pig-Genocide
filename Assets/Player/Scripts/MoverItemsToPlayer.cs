using VContainer;
using UnityEngine;

public class MoverItemsToPlayer : MonoBehaviour
{
    [Inject]
    public void Construct(IPlayerProvider playerProvider)
    {
        playerProvider.OnPlayerSpawned += OnPlayerSpawned;
        
        if (playerProvider.Player != null)
            OnPlayerSpawned(playerProvider.Player);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        transform.position = player.transform.position;
    }
}
