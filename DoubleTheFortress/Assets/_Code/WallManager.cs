using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WallManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Wall _mywallScript;
    [SerializeField] private WallScriptableObject _ghost;
    [SerializeField] private WallScriptableObject _basic;
    [SerializeField] private WallScriptableObject _upgradeOne;
    [SerializeField] private WallScriptableObject _upgradeTwo;

    private List<WallScriptableObject> _wallsList = new List<WallScriptableObject>();
    private WallScriptableObject _currentWall;
    private GameObject _currentWallObject;
    private IConstructable _mywall;
    private int _wallIndex;


    private void Start()
    {
        Prepare();
    }

    public void ReceiveHammer(float repairPoints, float upgradePoints)
    {
        if(_mywall.CurrentHealth < _mywall.MaxHealth)
        {
            _mywall.Repair(repairPoints);
            Debug.Log("Repaired life to " + _mywall.CurrentHealth + " out of " + _mywall.MaxHealth);
        } else if ( _mywall.CurrentHealth >= _mywall.MaxHealth && _mywall.UpgradePoints < _mywall.UpgradePointsRequired)
        {
            _mywall.AddUpgradePoints(upgradePoints);
            Debug.Log("Upgraded " + _mywall.UpgradePoints + " out of " + _mywall.UpgradePointsRequired);
        } else if (_mywall.CurrentHealth >= _mywall.MaxHealth && _mywall.UpgradePoints >= _mywall.UpgradePointsRequired)
        {
            _mywall.Upgrade(_mywall.CurrentObject, _wallsList[GetCurrentIndex() + 1].Model);
            Debug.Log("Upgraded to new Model");
        }
    }

    public void UpgradeSuccess()
    {
        ChangeCurrentWall();
        NewWall(_currentWall);
        Debug.Log("You got upgraded");
    }

    private void NewWall(WallScriptableObject currentWall)
    {
        _mywall.CurrentHealth = currentWall.HealthPool / 2; //for testing only
        //_mywall.CurrentHealth = currentWall.HealthPool;
        _mywall.MaxHealth = currentWall.HealthPool;
        _mywall.UpgradePoints = 0f;
        _mywall.UpgradePointsRequired = currentWall.UpgradeCost;
    }

    private void ChangeCurrentWall()
    {
        _wallIndex++;
        _currentWall = _wallsList[_wallIndex];
        _currentWallObject = _mywall.CurrentObject;
    }

    private int GetCurrentIndex()
    {
        return _wallsList.IndexOf(_currentWall);
    }

    private void Prepare()
    {
        _mywall = _mywallScript.GetComponent<IConstructable>();
        _wallsList.Add(_ghost);
        _wallsList.Add(_basic);
        _wallsList.Add(_upgradeOne);
        _wallsList.Add(_upgradeTwo);
        _currentWall = _wallsList[0];
        _wallIndex = 0;
        NewWall(_currentWall);
        _currentWallObject = _currentWall.Model;
        _mywall.Build(_currentWallObject,Vector3.zero, Quaternion.identity);
    }
}
