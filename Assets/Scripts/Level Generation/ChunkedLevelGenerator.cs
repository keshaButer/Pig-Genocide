using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkedLevelGenerator : MonoBehaviour
{
    public static ChunkedLevelGenerator SingleTon;

    private void Awake()
    {
        if (SingleTon == null)
            SingleTon = this;
        else if (SingleTon != null)
            Destroy(this);
    }

    [Header("Настройки")]
    public int chunkSize = 20;               // размер чанка в тайлах
    public int worldWidthInChunks = 5;        // сколько чанков по X
    public int worldHeightInChunks = 3;       // сколько чанков по Y
    public TileBase platformTile;              // тайл платформы
    public GameObject chunkPrefab;             // префаб с Tilemap, CompositeCollider и т.д.
    [SerializeField] private float noiseScale = 20;
    [SerializeField] private float porog = 0.5f;
    [SerializeField] private float cellSize = 0.3f;

    private Dictionary<Vector2Int, Tilemap> chunks = new Dictionary<Vector2Int, Tilemap>();

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        for (int cx = 0; cx < worldWidthInChunks; cx++)
        {
            for (int cy = 0; cy < worldHeightInChunks; cy++)
            {
                // Создаём чанк
                GameObject chunkObj = Instantiate(chunkPrefab, transform);
                chunkObj.name = $"Chunk_{cx}_{cy}";
                chunkObj.transform.position = new Vector3(cx * chunkSize * cellSize, cy * chunkSize * cellSize, 0);

                Tilemap tilemap = chunkObj.GetComponentInChildren<Tilemap>();
                chunks[new Vector2Int(cx, cy)] = tilemap;

                // Генерируем тайлы внутри этого чанка (можно PerlinNoise)
                for (int x = 0; x < chunkSize; x++)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        int worldX = cx * chunkSize + x;
                        int worldY = cy * chunkSize + y;
                        // Здесь твоя логика генерации (шум, острова и т.п.)
                        if (ShouldPlaceTile(worldX, worldY))
                        {
                            tilemap.SetTile(new Vector3Int(x, y, 0), platformTile);
                        }
                    }
                }
            }
        }
    }

    bool ShouldPlaceTile(int x, int y)
    {
        float noiseValue = Mathf.PerlinNoise(x / noiseScale, y / noiseScale);
        if (noiseValue > porog)
        {
            return true;
        }

        return false;
    }

    public void DestroyTileAtWorldCell(Vector3 worldPos)
    {
        int cellX = Mathf.FloorToInt(worldPos.x / cellSize);
        int cellY = Mathf.FloorToInt(worldPos.y / cellSize);

        int chunkX = Mathf.FloorToInt((float)cellX / chunkSize);
        int chunkY = Mathf.FloorToInt((float)cellY / chunkSize);
        Vector2Int chunkKey = new Vector2Int(chunkX, chunkY);

        if (chunks.TryGetValue(chunkKey, out Tilemap chunkTilemap))
        {
            Vector3Int localCell = new Vector3Int(
                cellX - chunkX * chunkSize,
                cellY - chunkY * chunkSize,
                0
            );
            chunkTilemap.SetTile(localCell, null);
        }
    }
}
