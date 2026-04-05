using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int maxAttempts = 20000;

    public static event Action<GameObject> OnPlayerSpawned;
    private void OnDestroy()
    {
        OnPlayerSpawned = null;
    }

    public Transform SpawnPlayer(List<Vector2> surfaceCells)
    {
        ChunkedLevelGenerator levelGenerator = ChunkedLevelGenerator.SingleTon;

        for (int attempts = 1; attempts < maxAttempts; attempts++)
        {
            Vector2 cell = surfaceCells[UnityEngine.Random.Range(0, surfaceCells.Count)];
            if (levelGenerator.IsFreeCell(cell))
            {
                levelGenerator.SetOccupiedCell(cell);

                GameObject playerObject = GameObject.Instantiate(
                    playerPrefab,
                    cell + new Vector2(0, 1),
                    Quaternion.Euler(0, 0, 0)
                );

                OnPlayerSpawned?.Invoke(playerObject);

                return playerObject.transform;
            }
        }
        return null;
    }
}
