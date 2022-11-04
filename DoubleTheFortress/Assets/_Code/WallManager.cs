using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WallManager : MonoBehaviour , IPause
{
    #region Configuration

    [Header("Dependencies")] 
    [SerializeField] private GameObject _cannonPrefab;
    [SerializeField] private Transform _cannonTransform;
    
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

    [Header("Settings")]
    [SerializeField] private Vector3 _instanciateRotationOffset;
    [SerializeField] private Vector3 _instanciatePositionOffset;
    [SerializeField] private Vector3 _instanciateScale;


    private List<WallScriptableObject> _wallsList = new List<WallScriptableObject>();
    private WallScriptableObject _currentWall;
    private GameObject _currentWallObject;
    private IConstructable _mywall;
    private int _wallIndex;
    private Collider _mainCollider;
    private GameObject _currentCannon;
    #endregion

    private void Start()
    {
        Prepare();
    }

    public void ReceiveHammer(float repairPoints, float upgradePoints)
    {
        if (_isPaused) return;
        if(_mywall.CurrentHealth < _mywall.MaxHealth)
        {
            _mywall.Repair(repairPoints);
            Debug.Log("Repaired life to " + _mywall.CurrentHealth + " out of " + _mywall.MaxHealth);
            return;
        }

        if (GetCurrentIndex() == 3) return;

        if (_mywall.UpgradePoints < _mywall.UpgradePointsRequired)
        {
            _mywall.AddUpgradePoints(upgradePoints);
            Debug.Log("Upgraded " + _mywall.UpgradePoints + " out of " + _mywall.UpgradePointsRequired);
            return;
        } 
        
        if (_mywall.CurrentHealth >= _mywall.MaxHealth && _mywall.UpgradePoints >= _mywall.UpgradePointsRequired)
        {
            _mywall.Upgrade(_mywall.CurrentObject, _wallsList[GetCurrentIndex() + 1].Model);
            Debug.Log("Upgraded to new Model");
        }
    }

    public void ReceiveDamage(GameObject sender, float damage)
    {
        if (_isPaused) return;
        if (GetCurrentIndex() == 0) return;
        if (_mywall.CurrentHealth > 0 && damage < _mywall.CurrentHealth)
        {
            //_mywallScript.ReceiveRayCaster(this.gameObject, damage);
            _mywall.CurrentHealth -= damage;
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
        _mywall.CurrentHealth = currentWall.HealthPool;
        _mywall.MaxHealth = currentWall.HealthPool;
        _mywall.UpgradePoints = 0f;
        _mywall.UpgradePointsRequired = currentWall.UpgradeCost;
    }

    private void UpgradeCurrentWall()
    {
        _wallIndex++;
        _currentWall = _wallsList[_wallIndex];
        _currentWallObject = _mywall.CurrentObject;
        UpdateTrigger();
        UpdateCannon();
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
        _mywall.Build(_currentWallObject, this.transform.position + _instanciatePositionOffset, Quaternion.Euler(_instanciateRotationOffset), this.gameObject, _instanciateScale);
        _mywallScript.onRecieveHammer += ReceiveHammer;
        _mywallScript.onRecieveDamage += ReceiveDamage;
        _mainCollider = this.gameObject.GetComponent<Collider>();
        UpdateTrigger();
        _isPaused = GameManager.Instance.IsPaused;
        _currentCannon = Instantiate(_cannonPrefab, _cannonTransform.position, _cannonTransform.rotation, _cannonTransform);
    }

    private void UpdateCannon()
    {
        if (GetCurrentIndex() ==0)
        {
            _currentCannon.SetActive(false);
        }
        else
        {
            _currentCannon.SetActive(true);
        }
    }

    private void DownGrade()
    {
        _wallIndex--;
        Destroy(_mywall.CurrentObject);
        _currentWall = _wallsList[_wallIndex];
        _mywall.CurrentObject = _currentWall.Model;
        _currentWallObject = _mywall.CurrentObject;
        _mywall.Build(_currentWallObject, this.transform.position + _instanciatePositionOffset, Quaternion.Euler(_instanciateRotationOffset), this.gameObject, _instanciateScale);
        NewWall(_currentWall);
        UpdateTrigger();
        UpdateCannon();
    }
    
    private void UpdateTrigger()
    {
        if(GetCurrentIndex() == 0)
        {
            _mainCollider.isTrigger = true;
        } else
        {
            _mainCollider.isTrigger = false;
        }
    }

    #region Interface Methods

    private bool _isPaused;
    public bool IsPaused { set => _isPaused = value; }

    void Pause()
    {
        _isPaused = true;
    }

    void Unpause()
    {
        _isPaused = false;
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.PauseGameEvent += Pause;
        GameManager.Instance.PlayGameEvent += Unpause;
    }

    private void UnsubscribeToEvents()
    {
        GameManager.Instance.PauseGameEvent -= Pause;
        GameManager.Instance.PlayGameEvent -= Unpause;
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }
    #endregion

}
