using Objects;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(SwitchPlatform))]
public class SwitchPlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SwitchPlatform switchPlatform = (SwitchPlatform)target;
        SpriteRenderer switchPlatformSpriteRenderer = switchPlatform.GetComponent<SpriteRenderer>();
        BoxCollider2D switchPlatformCollider = switchPlatform.spBoxCollider;
        serializedObject.ApplyModifiedProperties();

        if(switchPlatform.spSize.x <= 2)
        {
            switchPlatform.spSize.x = 2;
        }

        if(switchPlatform.spSize.y <= 2)
        {
            switchPlatform.spSize.y = 2;
        }

        if(switchPlatformSpriteRenderer != null)
        {
            switchPlatformSpriteRenderer.size = switchPlatform.spSize;
        }

        if(switchPlatformCollider != null)
        {
            switchPlatformCollider.size = switchPlatform.spSize - Vector2.one * 1;
        }
    }
}
