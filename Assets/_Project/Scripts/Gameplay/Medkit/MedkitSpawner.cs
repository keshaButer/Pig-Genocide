using VContainer;
using VContainer.Unity;
using System.Collections.Generic;
using UnityEngine;

public class MedkitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject medkitPrefab;
    [SerializeField] private float minDistanceBetweenMedkits = 5;
    [SerializeField] private int count;
    [SerializeField] private int maxAttempts = 200000;
    [SerializeField] private float distanceToDisable = 50;
    [SerializeField] private float updateActivationRate = 2;

    private List<GameObject> medkits = new List<GameObject>();
    private Transform playerTransform;
    [Inject] private ILevelGenerator levelGenerator;
    [Inject] private IInvokerFactory _invokerFactory;
    [Inject] private IObjectResolver _objectResolver;
    private IInvoker _invokerUpdateActivation;

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

        _invokerUpdateActivation = _invokerFactory.StartRepeatInvoking(
            updateActivationRate,
            UpdateActivationBarrels,
            this);
    }

    private void OnEnable()
    {
        if (_invokerUpdateActivation != null)
            _invokerUpdateActivation.Start();
    }

    private void OnDisable()
    {
       if (_invokerUpdateActivation != null)
            _invokerUpdateActivation.Stop();
    }

    public void SpawnBarrels(List<Vector2> surfaceCells)
    {
        int spawnedCount = 0;

        for (int attempts = 0; attempts < maxAttempts; attempts++)
        {
            Vector2 cell = surfaceCells[Random.Range(0, surfaceCells.Count)];

            if (levelGenerator.IsFreeCell(cell) && levelGenerator.IsDistanceSuitable(cell, minDistanceBetweenMedkits))
            {
                levelGenerator.SetOccupiedCell(cell);

                medkits.Add(_objectResolver.Instantiate(medkitPrefab, // OBJERT RESOLVER
                 cell + Vector2.up * 0.6f, Quaternion.Euler(0, 0, 0), transform));

                spawnedCount++;
                if (spawnedCount >= count)
                    break;
            }
        }
    }

    private void UpdateActivationBarrels()
    {
        foreach (GameObject medkit in medkits)
        {
            if (medkit != null)
            {
                bool shouldDisable = Vector2.Distance(playerTransform.position, medkit.transform.position) <= distanceToDisable;
                medkit.SetActive(shouldDisable);
            }
        }
    }
}
