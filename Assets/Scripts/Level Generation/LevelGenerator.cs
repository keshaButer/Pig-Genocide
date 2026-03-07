using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator SingleTon;

    private void Awake()
    {
        if (SingleTon == null)
            SingleTon = this;
        else if (SingleTon != null)
            Destroy(this);
    }

    [Header("Настройки сеточки")]
    [Range(1, 2000)]
    [SerializeField] private int _width = 20;
    [Range(1, 2000)]
    [SerializeField] private int _height = 10;

    [Header("Настройки шума")]
    [SerializeField] private float _noiseScale = 5f;
    [SerializeField] private float _porog;
    [SerializeField] private float _terreinHeight;

    [Header("Tile Map")]
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _platformTile;

    private bool[,] _walkableGrid;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        _tilemap.ClearAllTiles();

        _walkableGrid = new bool[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                float noiseValue = Mathf.PerlinNoise(x / _noiseScale, y / _noiseScale);

                bool isWalkableCell = noiseValue > _porog;
                _walkableGrid[x, y] = isWalkableCell;

                if (isWalkableCell)
                {
                    _tilemap.SetTile(new Vector3Int(x, y, 0), _platformTile);
                }
            }
        }
    }

    // private void GenerateLevel()
    // {
    //     _tilemap.ClearAllTiles();
    //
    //     _walkableGrid = new bool[_width, _height];
    //
    //     for (int x = 0; x < _width; x++)
    //     {
    //         float noiseValue = Mathf.PerlinNoise(x / _noiseScale, x / _noiseScale);
    //         float surfaceHeight = Mathf.RoundToInt(noiseValue * _terreinHeight);
    //
    //         for (int y = 0; y <= surfaceHeight; y++)
    //         {
    //             _walkableGrid[x, y] = true;
    //             _tilemap.SetTile(new Vector3Int(x, y, 0), _platformTile);
    //         }
    //     }
    // }

    public bool IsWalkableCell(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
            return _walkableGrid[x, y];

        return false;
    }

    public bool IsWalkable(Vector3 worldPos)
    {
        Vector3Int cellPos = _tilemap.WorldToCell(worldPos);
        return IsWalkableCell(cellPos.x, cellPos.y);
    }
}
