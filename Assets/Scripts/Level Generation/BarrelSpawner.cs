using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private float minDistanceBetweenBarrels = 5;
    [SerializeField] private int count;

    public void SpawnBarrels(List<Vector2> freeCells)
    {
        Debug.Log($"free cells count: {freeCells.Count}");

        ChunkedLevelGenerator levelGenerator = ChunkedLevelGenerator.SingleTon;
        for (int i = 0; i < count; i++)
        {
            Vector2 cell = freeCells[Random.Range(0, freeCells.Count)];
            if (levelGenerator.IsFreeCell(cell) && levelGenerator.IsDistanceSuitable(cell, minDistanceBetweenBarrels))
            {
                levelGenerator.SetOccupiedCell(cell);

                GameObject.Instantiate(barrelPrefab, cell + new Vector2(0.3f, 0.6f), Quaternion.Euler(0, 0, 0), transform);
                Debug.Log($"SPAWN BARREL IN: {cell}");
            }
        }
    }
}
