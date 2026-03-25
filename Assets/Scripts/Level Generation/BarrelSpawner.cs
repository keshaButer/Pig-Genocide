using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private float minDistanceBetweenBarrels = 5;
    [SerializeField] private int count;
    [SerializeField] private int maxAttempts = 200000;
    [SerializeField] private float distanceToDisable = 50;
    [SerializeField] private float disableFarRate = 2;

    private List<GameObject> barrels = new List<GameObject>();
    private Transform playerTransform;

    private void OnEnable() => PlayerSpawner.OnPlayerSpawned += Initialize;
    private void OnDisable() => PlayerSpawner.OnPlayerSpawned -= Initialize;

    public void Initialize(GameObject player)
    {
        playerTransform = player.transform;

        InvokeRepeating(nameof(DisableFarBarrels), 0, disableFarRate);
    }
    public void SpawnBarrels(List<Vector2> surfaceCells)
    {
        Debug.Log($"free cells count: {surfaceCells.Count}");

        ChunkedLevelGenerator levelGenerator = ChunkedLevelGenerator.SingleTon;
        int spawnedCount = 0;

        for (int attempts = 0; attempts < maxAttempts; attempts++)
        {
            Vector2 cell = surfaceCells[Random.Range(0, surfaceCells.Count)];

            if (levelGenerator.IsFreeCell(cell) && levelGenerator.IsDistanceSuitable(cell, minDistanceBetweenBarrels))
            {
                levelGenerator.SetOccupiedCell(cell);

                barrels.Add(GameObject.Instantiate(barrelPrefab, 
                 cell + Vector2.up * 0.6f, Quaternion.Euler(0, 0, 0), transform));

                spawnedCount++;
                if (spawnedCount >= count)
                    break;
            }
        }
        Debug.Log($"Barrels was spawned: {spawnedCount}");
    }
    void DisableFarBarrels()
    {
        foreach (GameObject barrel in barrels)
        {
            if (barrel != null)
            {
                bool shouldDisable = Vector2.Distance(playerTransform.position, barrel.transform.position) <= distanceToDisable;
                barrel.SetActive(shouldDisable);
            }
        }
    }
}
