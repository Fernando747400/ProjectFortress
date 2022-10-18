using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(WallManager))]
public class UITester : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WallManager wallManager = (WallManager)target;
        if (GUILayout.Button("Hit with hammer"))
        {
            wallManager.ReceiveHammer(10,15);
        }

        if (GUILayout.Button("Hit with raycaster"))
        {
            Debug.Log("Sent ray");
            wallManager.ReceiveDamage(new GameObject(),15f);
        }
    }
}
