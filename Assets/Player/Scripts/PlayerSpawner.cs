using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int maxAttempts = 200000;

    public Transform SpawnPlayer(List<Vector2> surfaceCells)
    {
        ChunkedLevelGenerator levelGenerator = ChunkedLevelGenerator.SingleTon;

        for (int attempts = 1; attempts < maxAttempts; attempts++)
        {
            Vector2 cell = surfaceCells[Random.Range(0, surfaceCells.Count)];
            if (levelGenerator.IsFreeCell(cell))
            {
                levelGenerator.SetOccupiedCell(cell);

                Debug.Log($"Player was spawned :)");
                return GameObject.Instantiate(playerPrefab, cell + new Vector2(0, 1), Quaternion.Euler(0, 0, 0)).transform;
            }
        }
        Debug.Log($"Player wasnt spawned");
        return null;
    }
}
