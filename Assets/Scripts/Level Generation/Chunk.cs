using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Chunk
{
    public Tilemap tilemap;
    public HashSet<Vector2Int> surfaceCells;
    public HashSet<Vector2Int> filledCells;
}
