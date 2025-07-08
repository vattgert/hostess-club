using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class Node
{
    public Vector2Int gridPos;
    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;
    public Node parent;
    public bool walkable;

    public Node(Vector2Int pos, bool walkable)
    {
        this.gridPos = pos;
        this.walkable = walkable;
    }
}

public class AStarPathfinder
{
    private Node[,] nodeGrid;
    private int width;
    private int height;
    private bool[,] walkableGrid;
    private Vector3Int gridOrigin;
    private Tilemap tilemap;
    // The cost of step from current node to any of its neighbors is always 1
    private readonly int STEP_COST = 1;

    public AStarPathfinder(PathfindingParams pathfinding)
    {
        walkableGrid = pathfinding.WalkableGrid;
        gridOrigin = pathfinding.GridOrigin;
        tilemap = pathfinding.Tilemap;

        width = walkableGrid.GetLength(0);
        height = walkableGrid.GetLength(1);
        nodeGrid = new Node[width, height];
        CreateMatrixNodesBasedOnWalkableGrid();
    }

    private void CreateMatrixNodesBasedOnWalkableGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodeGrid[x, y] = new Node(new Vector2Int(x, y), walkableGrid[x, y]);
            }
        }
    }

    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3Int cell = tilemap.WorldToCell(worldPos);
        return new Vector2Int(cell.x - gridOrigin.x, cell.y - gridOrigin.y);
    }

    private bool IsValid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance
    }

    private Node GetLowestFCostNode(List<Node> list)
    {
        Node best = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].fCost < best.fCost || (list[i].fCost == best.fCost && list[i].hCost < best.hCost))
                best = list[i];
        }
        return best;
    }

    private List<Vector3> ReconstructPath(Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node current = endNode;

        while (current != null)
        {
            Vector3 worldPos = tilemap.GetCellCenterWorld(new Vector3Int(
                current.gridPos.x + gridOrigin.x,
                current.gridPos.y + gridOrigin.y, 0));

            path.Add(worldPos);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    private List<Vector2Int> Neighbors()
    {
        return new List<Vector2Int>
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };
    }

    public List<Vector3> FindPath(Vector3 worldStart, Vector3 worldEnd)
    {
        // Convert start and end from world coordinates to grid coordinates
        Vector2Int start = WorldToGrid(worldStart);
        Vector2Int end = WorldToGrid(worldEnd);

        // Check if they are valid
        if (!IsValid(start) || !IsValid(end))
        {
            return null;
        }

        // Two lists: open for current node and probably nodes which are around it
        // and closed is for nodes which were processed
        List<Node> openList = new List<Node>();
        // Just for a faster "Contains" call
        HashSet<Node> openSet = new HashSet<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        // Take start node and end node references from grid
        Node startNode = nodeGrid[start.x, start.y];
        Node endNode = nodeGrid[end.x, end.y];

        startNode.gCost = 0;
        startNode.hCost = Heuristic(start, end);
        openList.Add(startNode);
        openSet.Add(startNode);
        while(openList.Count > 0)
        {
            Node current = GetLowestFCostNode(openList);
            //Debug.Log(current.gridPos.ToString() + " " + current.gCost + " " + current.hCost + " " + current.walkable);
            if (current == endNode)
            {
                return ReconstructPath(current);
            }

            // Remove current node from list and put it into "processed nodes" list
            openList.Remove(current);
            openSet.Remove(current);
            closedSet.Add(current);

            // Checking all the neighbours of the current node
            // by coordinates x + (1;-1) and y + (1;-1)v 
            foreach (Vector2Int offset in Neighbors())
            {
                Vector2Int neighborPos = current.gridPos + offset;
                // Check if heighbour position is valid
                if (!IsValid(neighborPos)) 
                {
                    continue;
                }
                // Check if neighbour is walkable and if we did not process it before
                Node neighbor = nodeGrid[neighborPos.x, neighborPos.y];
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                // Trying to estimate a cost of moving to the neighbor
                int tentativeG = current.gCost + STEP_COST;
                if (tentativeG < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = tentativeG;
                    neighbor.hCost = Heuristic(neighbor.gridPos, end);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }
}