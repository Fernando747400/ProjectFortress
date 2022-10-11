using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WallManager : MonoBehaviour
{
    #region Configuration
    [Header("Dependencies")]

    [Header("Wall Script")]
    [SerializeField] private Wall _mywallScript;

    [Header("Walls Models")]
    [SerializeField] private WallScriptableObject _ghost;
    [SerializeField] private WallScriptableObject _basic;
    [SerializeField] private WallScriptableObject _upgradeOne;
    [SerializeField] private WallScriptableObject _upgradeTwo;

    [Header("Colliders")]
    [SerializeField] private Collider _wallCollider;
    [SerializeField] private Rigidbody _wallRigidBody;

    [Header("Status Bars")]
    [SerializeField] private InformationBar _healthBar;
    [SerializeField] private InformationBar _upgradeBar;

    [Header("Settings")]
    [SerializeField] private Vector3 _instanciateRotationOffset;


    private List<WallScriptableObject> _wallsList = new List<WallScriptableObject>();
    private WallScriptableObject _currentWall;
    private GameObject _currentWallObject;
    private IConstructable _mywall;
    private int _wallIndex;
    #endregion

    private void Start()
    {
        Prepare();
    }

    public void ReceiveHammer(float repairPoints, float upgradePoints)
    {
        if(_mywall.CurrentHealth < _mywall.MaxHealth)
        {
            _mywall.Repair(repairPoints);
            _healthBar.UpdateBar(_mywall.CurrentHealth, _mywall.MaxHealth);
            Debug.Log("Repaired life to " + _mywall.CurrentHealth + " out of " + _mywall.MaxHealth);
            return;
        }

        if (GetCurrentIndex() == 3) return;

        if (_mywall.UpgradePoints < _mywall.UpgradePointsRequired)
        {
            _mywall.AddUpgradePoints(upgradePoints);
            _upgradeBar.UpdateBar(_mywall.UpgradePoints, _mywall.UpgradePointsRequired);
            Debug.Log("Upgraded " + _mywall.UpgradePoints + " out of " + _mywall.UpgradePointsRequired);
            return;
        } 
        
        if (_mywall.CurrentHealth >= _mywall.MaxHealth && _mywall.UpgradePoints >= _mywall.UpgradePointsRequired)
        {
            _mywall.Upgrade(_mywall.CurrentObject, _wallsList[GetCurrentIndex() + 1].Model);
            Debug.Log("Upgraded to new Model");
        }
    }

    public void ReceiveDamage(float damage)
    {
        if (GetCurrentIndex() == 0) return;
        if (_mywall.CurrentHealth > 0)
        {
            _mywallScript.ReceiveRayCaster(this.gameObject, damage);
            _healthBar.UpdateBar(_mywall.CurrentHealth, _mywall.MaxHealth);
            Debug.Log("Reduced life to " + _mywall.CurrentHealth + " out of " + _mywall.MaxHealth);
            return;
        }

        DownGrade();
        Debug.Log("Downgraded wall");
    }

    public void UpgradeSuccess()
    {
        UpgradeCurrentWall();
        NewWall(_currentWall);
        Debug.Log("You got upgraded");
    }

    private void NewWall(WallScriptableObject currentWall)
    {
        //_mywall.CurrentHealth = currentWall.HealthPool / 2; //for testing only
        _mywall.CurrentHealth = currentWall.HealthPool;
        _mywall.MaxHealth = currentWall.HealthPool;
        _mywall.UpgradePoints = 0f;
        _mywall.UpgradePointsRequired = currentWall.UpgradeCost;
        UpdateBars();
    }

    private void UpgradeCurrentWall()
    {
        _wallIndex++;
        _currentWall = _wallsList[_wallIndex];
        _currentWallObject = _mywall.CurrentObject;
        UpdateBars();
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
        _mywall.Build(_currentWallObject,this.transform.position, Quaternion.Euler(_instanciateRotationOffset));
        UpdateBars();
    }

    private void UpdateBars()
    {
        _healthBar.UpdateBar(_mywall.CurrentHealth, _mywall.MaxHealth);
        _upgradeBar.UpdateBar(_mywall.UpgradePoints, _mywall.UpgradePointsRequired);
    }

    private void DownGrade()
    {
        _wallIndex--;
        Destroy(_mywall.CurrentObject);
        _currentWall = _wallsList[_wallIndex];
        _mywall.CurrentObject = _currentWall.Model;
        _currentWallObject = _mywall.CurrentObject;
        _mywall.Build(_currentWallObject, this.transform.position, Quaternion.Euler(_instanciateRotationOffset));
        NewWall(_currentWall);
        UpdateBars();
    }

}
