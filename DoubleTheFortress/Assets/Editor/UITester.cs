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
            wallManager.ReceiveHammer(10,5);
        }

        if (GUILayout.Button("Hit with raycaster"))
        {
            Debug.Log("Sent ray");
            wallManager.ReceiveDamage(15);
        }
    }
}
