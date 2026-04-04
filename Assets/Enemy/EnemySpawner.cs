using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float spawnRate = 6;
    [SerializeField] private float minDistanceBetweenEnemies = 5;
    [SerializeField] private float minDistanceToPlayer = 5;
    [SerializeField] private float maxDistanceToPlayer = 10;
    [SerializeField] private float distanceToDisable = 50;
    [SerializeField] private float spawnHeightOffset = 0.6f;
    [SerializeField] private int maxAttempts = 20000;

    [Header("Other Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private Transform parentObject;
    [SerializeField] private float updateEnemyActivationRate = 2;

    private List<GameObject> enemies = new List<GameObject>();
    private Transform playerTransform;
    private Coroutine spawnRepeatCoroutine;
    private bool doSpawn;
    private ChunkedLevelGenerator levelGenerator;

    public void Subscribe() => PlayerSpawner.OnPlayerSpawned += Initialize;
    private void OnDisable() => PlayerSpawner.OnPlayerSpawned -= Initialize;

    public void Initialize(GameObject player)
    {
        playerTransform = player.transform;
        levelGenerator = ChunkedLevelGenerator.SingleTon;

        InvokeRepeating(nameof(UpdateEnemyActivation), 0, updateEnemyActivationRate);

        doSpawn = true;
        spawnRepeatCoroutine = StartCoroutine(nameof(SpawnRepeat), spawnRate);
    }

    private IEnumerator SpawnRepeat(float rate)
    {
        while (doSpawn)
        {
            SpawnEnemyInRadius();

            yield return new WaitForSeconds(rate);
        }
    }
    private void SpawnEnemyInRadius()
    {
        for (int attempts = 1; attempts < maxAttempts; attempts++)
        {
            Vector2 cell = levelGenerator.GetRandomSurfaceTileInRadius(playerTransform.position, maxDistanceToPlayer);

            float distanceToPlayer = Vector2.Distance(playerTransform.position, cell);
            bool farEnoughToPlayer = distanceToPlayer >= minDistanceToPlayer;

            if (farEnoughToPlayer)
            {
                levelGenerator.SetOccupiedCell(cell);

                GameObject enemy = GameObject.Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], 
                 cell + Vector2.up * spawnHeightOffset, Quaternion.Euler(0, 0, 0), parentObject);
                enemy.GetComponent<EnemyRasher>().Initialize(playerTransform.gameObject);
                enemies.Add(enemy);

                break;
            }
        }
    }
    private void UpdateEnemyActivation()
    {
        float sqrDintanceToDisable = distanceToDisable * distanceToDisable;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            float sqrDist = (playerTransform.position - enemies[i].transform.position).sqrMagnitude;
            bool shouldActivate = sqrDist < sqrDintanceToDisable;

            if (enemies[i].activeSelf != shouldActivate)
            {
                enemies[i].SetActive(shouldActivate);
            }
        }
    }
}
