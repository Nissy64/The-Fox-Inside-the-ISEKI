using UnityEngine;
using UnityEditor;

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
