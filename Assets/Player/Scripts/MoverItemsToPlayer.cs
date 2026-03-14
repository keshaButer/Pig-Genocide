using UnityEngine;

public class MoverItemsToPlayer : MonoBehaviour
{
    private void Awake()
    {
        MovementPlayer.OnPlayerSpawned += MoveItemsToPlayer;
    }
    private void MoveItemsToPlayer()
    {
        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
    private void OnDisable()
    {
        MovementPlayer.OnPlayerSpawned -= MoveItemsToPlayer;
    }
    private void OnDestroy()
    {
        MovementPlayer.OnPlayerSpawned -= MoveItemsToPlayer;
    }
}
