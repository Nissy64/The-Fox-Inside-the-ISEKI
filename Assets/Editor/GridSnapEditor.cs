using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridSnap))]
[CanEditMultipleObjects]
public class GridSnapEditor : Editor
{
    private GridSnap[] _instances;
    private Vector3 _center = Vector3.zero;

    private void OnEnable()
    {
        _instances = targets.Cast<GridSnap>().ToArray();
    }

    private void OnSceneGUI()
    {
        Tools.current = Tool.None;

        _center = GetCenterOfInstances(_instances);

        AxisHandle(Color.red, Vector2Int.right);

        AxisHandle(Color.green, Vector2Int.up);
    }

    private static Vector3 GetCenterOfInstances(GridSnap[] instances)
    {
        float x = 0f, y = 0f;

        foreach (var ins in instances)
        {
            var position = ins.transform.position;
            x += position.x;
            y += position.y;
        }

        return new Vector3(x / instances.Length, y / instances.Length, 0);
    }

    private void AxisHandle(Color color, Vector2 direction)
    {
        Handles.color = color;
        EditorGUI.BeginChangeCheck();
        var deltaMovement = Handles.Slider(_center, new Vector3(direction.x, direction.y, 0)) - _center;

        if (EditorGUI.EndChangeCheck())
        {
            var dot = Vector2.Dot(deltaMovement, direction);
            if (!(Mathf.Abs(dot) > Mathf.Epsilon)) return;

            MoveObject(dot * direction);
        }
    }

    private void MoveObject(Vector3 vec3)
    {
        var vec2 = new Vector2Int(Mathf.RoundToInt(vec3.x / 1), Mathf.RoundToInt(vec3.y / 1));

        if (vec2 == Vector2.zero) return;

        foreach (var ins in _instances)
        {
            Object[] objects = {ins, ins.transform};
            Undo.RecordObjects(objects, "オブジェクトの移動");
            ins.Move(vec2);
        }
    }
}