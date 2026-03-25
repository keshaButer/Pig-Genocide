using UnityEngine;

public class MoverItemsToPlayer : MonoBehaviour
{
    private void OnEnable() => PlayerSpawner.OnPlayerSpawned += MoveItemsToPlayer;
    private void OnDisable() => PlayerSpawner.OnPlayerSpawned -= MoveItemsToPlayer;

    private void MoveItemsToPlayer(GameObject player)
    {
        transform.position = player.transform.position;
    }
}
