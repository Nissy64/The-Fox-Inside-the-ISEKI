using Objects;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(PressRock))]
public class PressRockEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PressRock pressRock = (PressRock)target;
        SpriteRenderer pressRockSpriteRenderer = pressRock.GetComponent<SpriteRenderer>();
        serializedObject.ApplyModifiedProperties();

        if(pressRock.pressRockHeight <= 0)
        {
            pressRock.pressRockHeight = 0;
        }

        if(pressRockSpriteRenderer != null)
        {
            pressRockSpriteRenderer.size = new Vector2(pressRockSpriteRenderer.size.x, pressRock.pressRockHeight);
        }
    }
}
