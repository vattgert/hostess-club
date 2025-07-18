using UnityEngine;
using UnityEngine.Tilemaps;

public struct PathfindingParams
{
    public Tilemap Tilemap { get; private set; }
    public bool[,] WalkableGrid { get; private set; }
    public Vector3Int GridOrigin { get; private set; }

    public PathfindingParams(Tilemap tilemap, bool[,] walkableGrid, Vector3Int gridOrigin)
    {
        Tilemap = tilemap;
        WalkableGrid = walkableGrid;
        GridOrigin = gridOrigin;
    }
}

public class TilemapGridBuilder : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap;
    [SerializeField]
    private LayerMask obstacleMask; // e.g. assign "Obstacles" layer for tables
    private bool[,] walkableGrid;
    private Vector3Int gridOrigin;
    private Vector3 cellSize;

    private void Start()
    {
        BuildGrid();
    }

    private void LogBuiltTilemapGrid()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int j = walkableGrid.GetLength(1) - 1; j >= 0; j--) // rows, top to bottom
        {
            for (int i = 0; i < walkableGrid.GetLength(0); i++)  // columns, left to right
            {
                sb.Append(walkableGrid[i, j] ? "1" : "0").Append(" ");
            }
            sb.AppendLine();
        }
        //Debug.Log(sb.ToString());
    }

    private void BuildGrid()
    {
        BoundsInt bounds = floorTilemap.cellBounds;
        Debug.Log("Actual size occupied: " + bounds.ToString());
        Debug.Log($"Bounds min: {bounds.min}, max: {bounds.max}, size: {bounds.size}");
        gridOrigin = bounds.min;
        cellSize = floorTilemap.cellSize;

        int width = bounds.size.x;
        int height = bounds.size.y;
        Debug.Log("Width " + width);
        Debug.Log("Height " + height);
        walkableGrid = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cell = new Vector3Int(gridOrigin.x + x, gridOrigin.y + y, 0);
                Vector3 worldCenter = floorTilemap.GetCellCenterWorld(cell);

                // If no tile, not walkable
                if (floorTilemap.GetTile(cell) == null)
                {
                    walkableGrid[x, y] = false;
                    continue;
                }

                // If something overlaps with collider, not walkable
                bool hasObstacle = Physics2D.OverlapBox(worldCenter, cellSize * 0.9f, 0f, obstacleMask);
                walkableGrid[x, y] = !hasObstacle;
            }
        }

        Debug.Log("Tilemap grid built!");
        LogBuiltTilemapGrid();
    }

    public bool[,] GetWalkableTilemapGrid()
    {
        return walkableGrid;
    }

    public PathfindingParams GetPathfindingParams()
    {
        return new PathfindingParams(floorTilemap, walkableGrid, gridOrigin);
    }
}