using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(Transform))]
public class SpiteFliperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Transform transform = (Transform)target;

        if(GUILayout.Button("Flip"))
        {
            transform.eulerAngles += new Vector3(0, 180, 0);
        }
    }
}
