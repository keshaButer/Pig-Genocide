using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestructionManager : MonoBehaviour
{
    private Tilemap tilemap;
    private List<Vector3Int> pendingDestructions = new List<Vector3Int>();

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void DestroyTileAt(Vector3Int cellPosition)
    {
        // Добавляем только если ещё не запланировано
        if (!pendingDestructions.Contains(cellPosition))
            pendingDestructions.Add(cellPosition);
    }

    private void LateUpdate()
    {
        if (pendingDestructions.Count == 0) return;

        foreach (var cell in pendingDestructions)
        {
            tilemap.SetTile(cell, null);
        }

        pendingDestructions.Clear();
    }
}
