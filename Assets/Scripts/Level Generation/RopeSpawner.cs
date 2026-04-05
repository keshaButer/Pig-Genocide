using System.Collections.Generic;
using UnityEngine;

public class RopeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ropePrefab;
    [SerializeField] private float minDistanceBetweenGrabers = 5;
    [SerializeField] private float maxDistanceBetweenGrabers = 15;
    [SerializeField] private float minDistanceBetweenRopes = 30;
    [SerializeField] private int spawnCount;
    [SerializeField] private int maxTotalAttempts = 20000;
    [SerializeField] private float distanceToDisable = 50;
    [SerializeField] private float disableFarRate = 2;

    private List<GameObject> ropes = new List<GameObject>();
    private Transform playerTransform;

    private void OnEnable() => PlayerSpawner.OnPlayerSpawned += Initialize;
    private void OnDisable() => PlayerSpawner.OnPlayerSpawned -= Initialize;

    public void Initialize(GameObject player)
    {
        playerTransform = player.transform;

        InvokeRepeating(nameof(DisableFarRopes), 0, disableFarRate);
    }

    public void SpawnRopes(List<Vector2> surfaceCells)
    {
        ChunkedLevelGenerator levelGenerator = ChunkedLevelGenerator.SingleTon;
        int spawnedCount = 0;
        int attempts = 0;

        while (spawnedCount < spawnCount && attempts < maxTotalAttempts)
        {
            attempts++;
            Vector2 firstPoint = surfaceCells[Random.Range(0, surfaceCells.Count)];

            if (levelGenerator.IsFreeCell(firstPoint) && levelGenerator.IsDistanceSuitable(firstPoint, minDistanceBetweenRopes))
            {
                Vector2 secondPoint = Vector2.zero;
                bool secondPointFound = false;

                for (int attempt = 0; attempt < 30; attempt++) 
                {
                    Vector2 potentialSecond = surfaceCells[Random.Range(0, surfaceCells.Count)];
                    float dist = Vector2.Distance(firstPoint, potentialSecond);

                    if (levelGenerator.IsFreeCell(potentialSecond) && dist >= minDistanceBetweenGrabers && dist <= maxDistanceBetweenGrabers)
                    {
                        secondPoint = potentialSecond;
                        secondPointFound = true;
                        break;
                    }
                }

                if (secondPointFound)
                {
                    levelGenerator.SetOccupiedCell(firstPoint);
                    levelGenerator.SetOccupiedCell(secondPoint);

                    GameObject rope = GameObject.Instantiate(ropePrefab, firstPoint, Quaternion.Euler(0, 0, 0), transform);
                    ropes.Add(rope);
                    rope.transform.GetChild(0).position = firstPoint + new Vector2(0, 1.5f);
                    rope.transform.GetChild(1).position = secondPoint + new Vector2(0, 1.5f);
                    
                    spawnedCount++;
                }
            }
        }
    }
    void DisableFarRopes()
    {
        foreach (GameObject rope in ropes)
        {
            bool shouldDisable = Vector2.Distance(playerTransform.position, rope.transform.position) <= distanceToDisable;
            rope.SetActive(shouldDisable);
        }
    }
}
