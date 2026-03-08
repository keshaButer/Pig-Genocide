using System.Collections.Generic;
using UnityEngine;

public class RopeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ropePrefab;
    [SerializeField] private float minDistanceBetweenGrabers = 5;
    [SerializeField] private float maxDistanceBetweenGrabers = 15;
    [SerializeField] private float minDistanceBetweenRopes = 30;
    [SerializeField] private int count;

    public void SpawnRopes(List<Vector2> freeCells)
    {
        Debug.Log($"free cells count: {freeCells.Count}");
        Vector2 firstPoint;
        Vector2 secondPoint;
        GameObject rope = null;

        ChunkedLevelGenerator levelGenerator = ChunkedLevelGenerator.SingleTon;
        for (int i = 0; i < count; i++)
        {
            firstPoint = freeCells[Random.Range(0, freeCells.Count)];
            if (levelGenerator.IsFreeCell(firstPoint) && levelGenerator.IsDistanceSuitable(firstPoint, minDistanceBetweenRopes))
            {
                levelGenerator.SetOccupiedCell(firstPoint);

                rope = GameObject.Instantiate(ropePrefab, firstPoint + new Vector2(0.3f, 0.6f), Quaternion.Euler(0, 0, 0), transform);
                rope.transform.GetChild(0).position = firstPoint + new Vector2(0, 1.5f);
                Debug.Log($"SPAWN FIRST GRABBER IN: {firstPoint}");
            }
            if (rope != null)
            {
                while (true)
                {
                    secondPoint = freeCells[Random.Range(0, freeCells.Count)];
                    float distance = Vector2.Distance(firstPoint, secondPoint);
                    if (levelGenerator.IsFreeCell(secondPoint) && distance > minDistanceBetweenGrabers && distance <= maxDistanceBetweenGrabers) 
                    {
                        levelGenerator.SetOccupiedCell(secondPoint);

                        rope.transform.GetChild(1).position = secondPoint + new Vector2(0, 1.5f);
                        Debug.Log($"SPAWN SECOND GRABBER IN: {secondPoint}");
                        break;
                    }
                }
            }
        }
    }
}
