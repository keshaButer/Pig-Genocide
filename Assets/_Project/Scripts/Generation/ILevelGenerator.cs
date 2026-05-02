using UnityEngine;

public interface ILevelGenerator
{
    void DestroyTileAtWorldPosition(Vector3Int cellPos);
    void DestroyTileAtWorldPosition(Vector3 worldPos);
    void DestroyTilesInRadius(Vector3 worldCenter, float radius);
    void SetOccupiedCell(Vector2 pos);

    Vector2Int GetChunkIndex(Vector2Int pos);
    Vector2Int WorldCellToIndex(Vector2 pos);

    Vector2 GetRandomSurfaceTileInRadius(Vector3 worldCenter, float radius);
    Vector2 IndexCellToWorld(Vector2Int pos);

    bool IsFreeCell(Vector2 pos);
    bool IsSurfaceCell(Vector2Int pos);
    bool IsSurfaceCellUnder(Vector2Int pos);
    bool IsFilledCell(Vector2Int pos);
    bool HasSurfaceBelow(Vector2Int pos, int maxDepth);
    bool IsSurfaceCellUnderAround(Vector2Int pos);
    bool IsDistanceSuitable<T>(Vector2 pos, float minDistance) where T : MonoBehaviour;

    public int GetOccupiedCellsCount();
} 