using UnityEngine;

[CreateAssetMenu(fileName = "Wall", menuName = "ScriptableObjects/Wall")]
public class WallScriptableObject : ScriptableObject
{
    public string Name;
    public int Level;
    public GameObject Model;
    public float HealthPool;
    public float UpgradeCost;
}
