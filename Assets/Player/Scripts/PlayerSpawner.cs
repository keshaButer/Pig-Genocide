using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int maxAttempts = 200000;

    public void SpawnPlayer(List<Vector2> surfaceCells)
    {
        ChunkedLevelGenerator levelGenerator = ChunkedLevelGenerator.SingleTon;
        int attempts = 0;

        for (int i = 0; attempts < maxAttempts; i++)
        {
            attempts++;
            Vector2 cell = surfaceCells[Random.Range(0, surfaceCells.Count)];
            if (levelGenerator.IsFreeCell(cell))
            {
                levelGenerator.SetOccupiedCell(cell);

                GameObject.Instantiate(playerPrefab, cell + new Vector2(0, 1), Quaternion.Euler(0, 0, 0));

                break;
            }
            else
            {
                i--;
            }
        }
        Debug.Log($"Player was spawned");
    }
}
