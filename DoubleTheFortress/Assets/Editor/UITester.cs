using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WallManager))]
public class UITester : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WallManager wallManager = (WallManager)target;
        if (GUILayout.Button("Hit with hammer"))
        {
            wallManager.ReceiveHammer(10,5);
        }
    }
}
