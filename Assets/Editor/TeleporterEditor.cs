using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Teleporter))]
public class TeleporterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Teleporter teleporter = (Teleporter)target;

        if(GUILayout.Button("Flip Portal"))
        {
            teleporter.transform.eulerAngles += new Vector3(0, 180, 0);
        }
    }
}
