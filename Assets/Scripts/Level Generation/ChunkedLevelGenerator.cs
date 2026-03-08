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
    [SerializeField] private int seed = 0;
    [SerializeField] private float noiseScale = 20;
    [SerializeField] private float porog = 0.5f;
    [SerializeField] private float cellSize = 0.3f;
    [SerializeField] private float disableChunkRate = 5;
    [SerializeField] private float distanceDisableChunk = 10;
    private float xOffset, yOffset;

    private Dictionary<Vector3, GameObject> chunkObjects = new Dictionary<Vector3, GameObject>();
    private Dictionary<Vector2Int, Tilemap> chunks = new Dictionary<Vector2Int, Tilemap>();

    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        SetRandomOffset();

        GenerateWorld();

        InvokeRepeating(nameof(DisableFarChunks), 0f, disableChunkRate);
    }
    void SetRandomOffset()
    {
        if (seed == 0)
            seed = Random.Range(1, 10000);
            
        Random.InitState(seed);
        xOffset = Random.Range(0f, 1000f);
        yOffset = Random.Range(0f, 1000f);
    }

    void DisableFarChunks()
    {
        if (playerTransform == null) return;

        Vector3 playerPos = playerTransform.position;

        foreach (var kvp in chunkObjects)
        {
            GameObject obj = kvp.Value;

            Vector2 chunkCenter =  obj.transform.position + Vector3.one * chunkSize * cellSize * 0.5f;
            float distance = Vector2.Distance(playerPos, chunkCenter);
            obj.SetActive(distance <= distanceDisableChunk);
        }
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
                Vector3 chunkObjPosition = new Vector3(cx * chunkSize * cellSize, cy * chunkSize * cellSize, 0);
                chunkObj.transform.position = chunkObjPosition;

                Tilemap tilemap = chunkObj.GetComponentInChildren<Tilemap>();
                chunks[new Vector2Int(cx, cy)] = tilemap;
                chunkObjects[chunkObjPosition] = chunkObj;

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
        float noiseValue = Mathf.PerlinNoise(x / noiseScale + xOffset, y / noiseScale + yOffset);
        if (noiseValue > porog)
        {
            return true;
        }

        return false;
    }

    public void DestroyTileAtWorldPosition(Vector3Int cellPos)
    {
        int chunkX = Mathf.FloorToInt((float)cellPos.x / chunkSize);
        int chunkY = Mathf.FloorToInt((float)cellPos.y / chunkSize);
        Vector2Int chunkKey = new Vector2Int(chunkX, chunkY);

        if (chunks.TryGetValue(chunkKey, out Tilemap chunkTilemap))
        {
            Vector3Int localCell = new Vector3Int(
                cellPos.x - chunkX * chunkSize,
                cellPos.y - chunkY * chunkSize,
                0
            );
            chunkTilemap.SetTile(localCell, null);
        }
    }
    public void DestroyTileAtWorldPosition(Vector3 worldPos)
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
    public void DestroyTilesInRadius(Vector3 worldCenter, float radius)
    {
        // Радиус в клетках (с учётом cellSize)
        int radiusInCells = Mathf.CeilToInt(radius / cellSize);
        
        // Центр в клеточных координатах
        int centerX = Mathf.FloorToInt(worldCenter.x / cellSize);
        int centerY = Mathf.FloorToInt(worldCenter.y / cellSize);

        for (int x = -radiusInCells; x <= radiusInCells; x++)
        {
            for (int y = -radiusInCells; y <= radiusInCells; y++)
            {
                // Проверяем, попадает ли клетка в круг (по мировым координатам)
                Vector3 cellWorldPos = new Vector3((centerX + x) * cellSize, (centerY + y) * cellSize, 0);
                if (Vector3.Distance(worldCenter, cellWorldPos) <= radius)
                {
                    Vector3Int cellPos = new Vector3Int(centerX + x, centerY + y);
                    DestroyTileAtWorldPosition(cellPos);
                }
            }
        }
    }
}
