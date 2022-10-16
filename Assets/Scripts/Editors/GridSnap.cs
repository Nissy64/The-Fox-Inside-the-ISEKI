using UnityEngine;

[SelectionBase]
public class GridSnap : MonoBehaviour
{
    private Vector2Int gridPos = Vector2Int.zero;
    private static int gridSize = 1;

    public void Move(Vector2Int vec2)
    {
        gridPos += vec2;
        transform.position = GetGlobalPosition(gridPos);
    }

    public static Vector3 GetGlobalPosition(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x * gridSize,
            gridPos.y * gridSize,
            0);
    }
}