using System;
using UnityEngine;
using UnityEngine.Events;

public interface IConstructable 
{

    float CurrentHealth
    {
        get;
        set;
    }

    float MaxHealth
    {
        get;
        set;
    }

    float UpgradePoints
    {
        get;
        set;
    }

    float UpgradePointsRequired
    {
        get;
        set;
    }

    GameObject CurrentObject
    {
        get;
        set;
    }

    UnityEvent IBuildEvent
    {
        get;
    }

    UnityEvent IRepairEvent
    {
        get;
    }

    UnityEvent IUpgradePointsEvent
    {
        get;
    }

    UnityEvent IUpgradeEvent
    {
        get;
    }

    void RecieveHammer(float repairValue, float upgradeValue)
    {
        
    }

    void Build(GameObject building, Vector3 position, Quaternion rotation)
    {
        CurrentObject = GameObject.Instantiate(building, position, rotation);
        CurrentObject.SetActive(true);
        IBuildEvent?.Invoke();
    }

    void Repair(float repairValue)
    {
        if (CurrentHealth < MaxHealth) CurrentHealth += repairValue;
        CurrentHealth = Math.Clamp(CurrentHealth, 0f, MaxHealth);
        IRepairEvent?.Invoke();
    }

    void AddUpgradePoints(float pointsValue)
    {
        if (UpgradePoints < UpgradePointsRequired) UpgradePoints += pointsValue;
        UpgradePoints = Mathf.Clamp(UpgradePoints, 0f, UpgradePointsRequired);
        IUpgradePointsEvent?.Invoke();
    }

    void Upgrade(GameObject oldBuilding, GameObject newBuilding)
    {
        //oldBuilding.SetActive(false);
        Build(newBuilding, oldBuilding.transform.position, oldBuilding.transform.rotation);
        GameObject.Destroy(oldBuilding);
        IUpgradeEvent?.Invoke();
    }

    void UpgradeMeshOnly(GameObject building, Mesh newMesh)
    {
        building.GetComponent<MeshFilter>().mesh = newMesh;
        IUpgradeEvent?.Invoke();
    }
}
