using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] float spawnRate = 5;
    [SerializeField] private float minDistanceBetweenEnemies = 5;
    [SerializeField] private float minDistanceToPlayer = 5;
    [SerializeField] private float distanceToDisable = 50;
    [SerializeField] private float spawnHeightOffset = 0.6f;
    [SerializeField] private int maxAttempts = 20000;

    [Header("Other Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private float updateEnemyActivationRate = 2;

    private List<GameObject> enemies = new List<GameObject>();
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating(nameof(UpdateEnemyActivation), 0, updateEnemyActivationRate);
        InvokeRepeating(nameof(SpawnEnemy), 0, spawnRate);
    }
    public void SpawnEnemy(List<Vector2> surfaceCells)
    {
        ChunkedLevelGenerator levelGenerator = ChunkedLevelGenerator.SingleTon;

        for (int attempts = 1; attempts < maxAttempts; attempts++)
        {
            Vector2 cell = surfaceCells[Random.Range(0, surfaceCells.Count)];

            bool farEnoughToPlayer = Vector2.Distance(playerTransform.position, cell) >= minDistanceToPlayer;

            if (levelGenerator.IsFreeCell(cell) && levelGenerator.IsDistanceSuitable(cell, minDistanceBetweenEnemies) && farEnoughToPlayer)
            {
                levelGenerator.SetOccupiedCell(cell);

                enemies.Add(GameObject.Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], 
                 cell + Vector2.up * spawnHeightOffset, Quaternion.Euler(0, 0, 0), transform));

                break;
            }
        }
    }
    private void UpdateEnemyActivation()
    {
        float sqrMinDistanceToPlayer = minDistanceToPlayer * minDistanceToPlayer;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            float sqrDist = (playerTransform.position - enemies[i].transform.position).sqrMagnitude;
            bool shouldActivate = sqrDist < sqrMinDistanceToPlayer;

            if (enemies[i].activeSelf != shouldActivate)
            {
                enemies[i].SetActive(shouldActivate);
            }
        }
    }
}
