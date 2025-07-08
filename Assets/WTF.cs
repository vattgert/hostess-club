using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilemapGridLabelDrawer : MonoBehaviour
{
    public Tilemap tilemap;
    public Color labelColor = Color.white;
    public Vector3 labelOffset = new Vector3(0.3f, 0.3f, 0);
    public LayerMask obstacleMask;

//    private void OnDrawGizmos()
//    {
//        if (tilemap == null) return;

//        BoundsInt bounds = tilemap.cellBounds;
//        foreach (Vector3Int pos in bounds.allPositionsWithin)
//        {
//            if (!tilemap.HasTile(pos)) continue;

//            Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
//            GUIStyle style = new GUIStyle();
//            style.normal.textColor = labelColor;
//            //style.alignment = TextAnchor.MiddleCenter;
//            style.fontSize = 12;

//#if UNITY_EDITOR
//            Handles.Label(worldPos, $"({pos.x},{pos.y})", style);
//#endif
//        }
//    }

    //private void OnDrawGizmos()
    //{
    //    if (tilemap == null) return;

    //    Gizmos.color = Color.green;
    //    BoundsInt bounds = tilemap.cellBounds;

    //    foreach (Vector3Int pos in bounds.allPositionsWithin)
    //    {
    //        if (true && !tilemap.HasTile(pos)) continue;

    //        Vector3 center = tilemap.GetCellCenterWorld(pos);
    //        Vector3 size = tilemap.cellSize;

    //        Gizmos.DrawWireCube(center, size);
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    if (tilemap == null) return;

    //    BoundsInt bounds = tilemap.cellBounds;

    //    foreach (Vector3Int pos in bounds.allPositionsWithin)
    //    {
    //        if (true && !tilemap.HasTile(pos)) continue;

    //        Vector3 center = tilemap.GetCellCenterWorld(pos);
    //        Vector3 size = tilemap.cellSize * 0.9f;

    //        // Check for obstacle
    //        bool hasObstacle = Physics2D.OverlapBox(center, size, 0f, obstacleMask);

    //        // Set color
    //        Gizmos.color = hasObstacle ? Color.red : Color.green;

    //        Gizmos.DrawWireCube(center, tilemap.cellSize);
    //    }
    //}
}