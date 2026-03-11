using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkedLevelGenerator : MonoBehaviour
{
    public static ChunkedLevelGenerator SingleTon;

    [Header("Настройки")]
    public int chunkSizeInCells = 20;               // размер чанка в тайлах
    public int worldWidthInChunks = 5;        // сколько чанков по X
    public int worldHeightInChunks = 3;       // сколько чанков по Y
    public TileBase platformTile;              // тайл платформы
    public GameObject chunkPrefab;             // префаб с Tilemap, CompositeCollider и т.д.
    public List<Vector2> surfaceCells = new List<Vector2>();

    [SerializeField] private int seed = 0;
    [SerializeField] private float noiseScale = 20;
    [SerializeField] private float porog = 0.5f;
    [SerializeField] private float cellSize = 0.3f;
    [SerializeField] private float disableChunkRate = 5;
    [SerializeField] private float distanceDisableChunk = 10;
    [SerializeField] private BarrelSpawner barrelSpawner;
    [SerializeField] private RopeSpawner ropeSpawner;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private EnemySpawner enemySpawner;

    private float xOffset, yOffset;
    private Dictionary<Vector3, GameObject> chunkObjects = new Dictionary<Vector3, GameObject>();
    private Dictionary<Vector2Int, Tilemap> chunks = new Dictionary<Vector2Int, Tilemap>();
    private HashSet<Vector2> occupiedCells = new HashSet<Vector2>();
    private Transform playerTransform;

    public void Initialize()
    {
        if (SingleTon == null)
            SingleTon = this;
        else if (SingleTon != null)
            Destroy(this);

        SetRandomNoiseOffset();

        GenerateWorld();

        SetFreeSurfaceCells();

        barrelSpawner.SpawnBarrels(surfaceCells);
        ropeSpawner.SpawnRopes(surfaceCells);
        playerTransform = playerSpawner.SpawnPlayer(surfaceCells);
        // enemySpawner.StartSpawnEnemies();

        InvokeRepeating(nameof(SetActivationChunks), 0f, disableChunkRate);
    }
    void SetRandomNoiseOffset()
    {
        if (seed == 0)
            seed = Random.Range(1, 10000);
            
        Random.InitState(seed);
        xOffset = Random.Range(0f, 1000f);
        yOffset = Random.Range(0f, 1000f);
    }

    void SetActivationChunks()
    {
        if (playerTransform == null) return;

        Vector3 playerPos = playerTransform.position;

        foreach (var kvp in chunkObjects)
        {
            GameObject obj = kvp.Value;

            Vector2 chunkCenter =  obj.transform.position + Vector3.one * chunkSizeInCells * cellSize * 0.5f;
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
                Vector3 chunkObjPosition = new Vector3(cx * chunkSizeInCells * cellSize, cy * chunkSizeInCells * cellSize, 0);
                chunkObj.transform.position = chunkObjPosition;

                Tilemap tilemap = chunkObj.GetComponentInChildren<Tilemap>();
                chunks[new Vector2Int(cx, cy)] = tilemap;
                chunkObjects[chunkObjPosition] = chunkObj;

                // Генерируем тайлы внутри этого чанка (можно PerlinNoise)
                for (int x = 0; x < chunkSizeInCells; x++)
                {
                    for (int y = 0; y < chunkSizeInCells; y++)
                    {
                        int worldX = cx * chunkSizeInCells + x;
                        int worldY = cy * chunkSizeInCells + y;
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
        int chunkX = Mathf.FloorToInt((float)cellPos.x / chunkSizeInCells);
        int chunkY = Mathf.FloorToInt((float)cellPos.y / chunkSizeInCells);
        Vector2Int chunkKey = new Vector2Int(chunkX, chunkY);

        if (chunks.TryGetValue(chunkKey, out Tilemap chunkTilemap))
        {
            Vector3Int localCell = new Vector3Int(
                cellPos.x - chunkX * chunkSizeInCells,
                cellPos.y - chunkY * chunkSizeInCells,
                0
            );
            chunkTilemap.SetTile(localCell, null);
        }
    }
    public void DestroyTileAtWorldPosition(Vector3 worldPos)
    {
        int cellX = Mathf.FloorToInt(worldPos.x / cellSize);
        int cellY = Mathf.FloorToInt(worldPos.y / cellSize);

        int chunkX = Mathf.FloorToInt((float)cellX / chunkSizeInCells);
        int chunkY = Mathf.FloorToInt((float)cellY / chunkSizeInCells);
        Vector2Int chunkKey = new Vector2Int(chunkX, chunkY);

        if (chunks.TryGetValue(chunkKey, out Tilemap chunkTilemap))
        {
            Vector3Int localCell = new Vector3Int(
                cellX - chunkX * chunkSizeInCells,
                cellY - chunkY * chunkSizeInCells,
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
    private void SetFreeSurfaceCells()
    {
        surfaceCells.Clear();

        for (int cx = 0; cx < worldWidthInChunks; cx++)
        {
            for (int cy = 0; cy < worldHeightInChunks; cy++)
            {
                Tilemap tilemap = chunks[new Vector2Int(cx, cy)];

                for (int x = 0; x < chunkSizeInCells; x++)
                {
                    for (int y = 0; y < chunkSizeInCells; y++)
                    {
                        Vector3Int localCell = new Vector3Int(x, y, 0);
                        Vector3Int aboveLocal = new Vector3Int(x, y + 1, 0);

                        // Есть ли тайл в этой клетке?
                        if (tilemap.GetTile(localCell) != null)
                        {
                            bool isAboveFree;

                            if (y + 1 >= chunkSizeInCells && cy != worldHeightInChunks - 1)
                            {
                                // Выход за границу чанка — надо проверить соседний чанк сверху
                                Tilemap tilemapAbove = chunks[new Vector2Int(cx, cy + 1)];
                                isAboveFree = !tilemapAbove.GetTile(new Vector3Int(x, 0, 0));
                            }
                            else
                            {
                                isAboveFree = tilemap.GetTile(aboveLocal) == null;
                            }

                            if (isAboveFree)
                            {
                                surfaceCells.Add(new Vector2(cx * chunkSizeInCells * cellSize + x * cellSize, cy * chunkSizeInCells * cellSize + y * cellSize));
                            }
                        }
                    }
                }
            }
        }
    }
    public void SetOccupiedCell(Vector2 pos)
    {
        occupiedCells.Add(pos);
    }
    public bool IsFreeCell(Vector2 pos)
    {
        return !occupiedCells.Contains(pos);
    }
    public bool IsDistanceSuitable(Vector2 pos, float minDistance)
    {
        return !Physics2D.OverlapCircle(pos, minDistance, LayerMask.GetMask("Occupied"));;
    }
}
