using UnityEngine;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float spawnRate = 6;
    [SerializeField] private float minDistanceBetweenEnemies = 5;
    [SerializeField] private float minDistanceToPlayer = 5;
    [SerializeField] private float maxDistanceToPlayer = 10;
    [SerializeField] private float distanceToDisable = 50;
    [SerializeField] private float spawnHeightOffset = 0.6f;
    [SerializeField] private int maxAttempts = 2000;

    [Header("Other Settings")]
    [SerializeField] private List<Enemy> enemyPrefabs;
    [SerializeField] private Transform parentObject;
    [SerializeField] private float updateEnemyActivationRate = 2;

    private List<Enemy> enemies = new();
    private Transform playerTransform;

    [Inject] private ILevelGenerator _levelGenerator; // надо будет перетащить в auto inject
    [Inject] private IDifficultyManager _difficultyManager;

    [Inject] private IInvokerFactory _invokerFactory;
    private IInvoker _invokerUpdateActivation;
    private IInvoker _invokerSpawnEnemies;
    
    [Inject] private IObjectResolver _objectResolver;

    private void OnEnable()
    {
        if (_invokerSpawnEnemies != null)
            _invokerSpawnEnemies.Start();

        if (_invokerUpdateActivation != null)
            _invokerUpdateActivation.Start();
    }

    private void OnDisable()
    {
        if (_invokerSpawnEnemies != null)
            _invokerSpawnEnemies.Stop();

        if (_invokerUpdateActivation != null)
            _invokerUpdateActivation.Stop();
    }

    [Inject]
    public void Construct(IPlayerProvider playerProvider)
    {
        playerProvider.OnPlayerSpawned += OnPlayerSpawned;
        
        if (playerProvider.Player != null)
            OnPlayerSpawned(playerProvider.Player);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        playerTransform = player.transform;

        _invokerUpdateActivation = _invokerFactory.StartRepeatInvoking(updateEnemyActivationRate, UpdateEnemyActivation, this);
        _invokerSpawnEnemies = _invokerFactory.StartRepeatInvoking(spawnRate, SpawnEnemyInRadius, this);
    }

    private void SpawnEnemyInRadius()
    {
        for (int attempts = 1; attempts < maxAttempts; attempts++)
        {
            Vector2 cell = _levelGenerator.GetRandomSurfaceTileInRadius(playerTransform.position, maxDistanceToPlayer);

            float distanceToPlayer = Vector2.Distance(playerTransform.position, cell);
            bool farEnoughToPlayer = distanceToPlayer >= minDistanceToPlayer;

            if (farEnoughToPlayer)
            {
                _levelGenerator.SetOccupiedCell(cell);

                Enemy enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                Enemy enemy = _objectResolver.Instantiate(enemyPrefab,
                 cell + Vector2.up * spawnHeightOffset, Quaternion.Euler(0, 0, 0), parentObject);
                
                enemy.SetDifficulty(_difficultyManager.CurrentDifficulty);

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

            if (enemies[i].gameObject.activeSelf != shouldActivate)
            {
                enemies[i].gameObject.SetActive(shouldActivate);
            }
        }
    }
}