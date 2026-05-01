using UnityEngine;
using System;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

public class PlayerSpawner : MonoBehaviour, IPlayerProvider
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int maxAttempts = 20000;
    
    [Inject] private ILevelGenerator levelGenerator;
    [Inject] private IObjectResolver _objectResolver;

    public GameObject Player { get; private set; }

    public event Action<GameObject> OnPlayerSpawned;

    private void OnDisable()
    {
        OnPlayerSpawned = null;
    }

    public Transform SpawnPlayer(List<Vector2> surfaceCells)
    {
        for (int attempts = 1; attempts < maxAttempts; attempts++)
        {
            Vector2 cell = surfaceCells[UnityEngine.Random.Range(0, surfaceCells.Count)];
            if (levelGenerator.IsFreeCell(cell))
            {
                levelGenerator.SetOccupiedCell(cell);

                GameObject playerObject = _objectResolver.Instantiate(
                    playerPrefab,
                    cell + new Vector2(0, 1),
                    Quaternion.Euler(0, 0, 0)
                );

                Player = playerObject;
                OnPlayerSpawned?.Invoke(Player);

                return playerObject.transform;
            }
        }
        Debug.LogError("COULD NOT SPAWN PLAYER");
        return null;
    }
}