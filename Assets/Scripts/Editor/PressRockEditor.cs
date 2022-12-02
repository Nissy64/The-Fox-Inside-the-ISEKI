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
        BoxCollider2D pressRockArmCollider = pressRock.armCollider2D;
        serializedObject.ApplyModifiedProperties();

        if(pressRock.pressRockHeight <= 0)
        {
            pressRock.pressRockHeight = 0;
        }

        if(pressRockSpriteRenderer != null)
        {
            pressRockSpriteRenderer.size = new Vector2(pressRockSpriteRenderer.size.x, pressRock.pressRockHeight);
        }

        if(pressRockArmCollider != null)
        {
            pressRockArmCollider.offset = new Vector2(0, pressRock.pressRockHeight / 2 + 1.5f);
            pressRockArmCollider.size = new Vector2(3.875f, pressRock.pressRockHeight - 3);
        }
    }
}
