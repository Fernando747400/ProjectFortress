using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour , IPause
{
    #region Configuration

    [Header("Dependencies")] 
    [SerializeField] private GameObject _cannonObject;
    [SerializeField] private string _cannonName;
    
    [Header("Wall Script")]
    [SerializeField] private Wall _mywallScript;
    [SerializeField] private ShieldUIManager _shieldsUI;

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
    
    [Header("AudioClips")]
     public AudioClip _buildSound;
     public AudioClip _destroySound;
     public AudioClip _damageSound;

    private List<WallScriptableObject> _wallsList = new List<WallScriptableObject>();
    private WallScriptableObject _currentWall;
    private GameObject _currentWallObject;
    private IConstructable _mywall;
    private int _wallIndex;
    private Collider _mainCollider;
    private GameObject _currentCannon;

    public int WallIndex { get => _wallIndex; }
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
            //Debug.Log("Repaired life to " + _mywall.CurrentHealth + " out of " + _mywall.MaxHealth);
            return;
        }

        if (GetCurrentIndex() == 3) return;

        if (_mywall.UpgradePoints < _mywall.UpgradePointsRequired)
        {
            _mywall.AddUpgradePoints(upgradePoints);
            //Debug.Log("Upgraded " + _mywall.UpgradePoints + " out of " + _mywall.UpgradePointsRequired);
            return;
        } 
        
        if (_mywall.CurrentHealth >= _mywall.MaxHealth && _mywall.UpgradePoints >= _mywall.UpgradePointsRequired)
        {
            _mywall.Upgrade(_mywall.CurrentObject, _wallsList[GetCurrentIndex() + 1].Model);
        }

        _shieldsUI.UpdateShields(GetCurrentIndex());
    }

    public void ReceiveDamage(GameObject sender, float damage)
    {
        if (_isPaused) return;
        if (GetCurrentIndex() == 0) return;
        if (_mywall.CurrentHealth > 0 && damage < _mywall.CurrentHealth)
        {
            _mywall.CurrentHealth -= damage;
            PlayAudio(_damageSound);
            return;
        }
        DownGrade();
        _shieldsUI.UpdateShields(GetCurrentIndex());
    }

    public void UpgradeSuccess()
    {
        UpgradeCurrentWall();
        NewWall(_currentWall);
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
        PlayAudio(_buildSound);
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
        UpdateCannon();
        _isPaused = GameManager.Instance.IsPaused;
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
        PlayAudio(_destroySound);
        UpdateTrigger();
        UpdateCannon();
    }
    private void UpdateCannon()
    {
        if (_currentCannon == null)
        {
            FindCannon();
        }
        if (_currentCannon == null) return;

        if (GetCurrentIndex() ==0)
        {
            _currentCannon.SetActive(false);
        }
        else
        {
            _currentCannon.SetActive(true);
        }
    }

    private void FindCannon()
    {
        _currentCannon = GameObject.Find(_cannonName);
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

    private void PlayAudio(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayAudio(clip, volume, this.transform.position);
    }

    #region Interface Methods

    private bool _isPaused;
    public bool IsPaused { set => _isPaused = value; }

    void Pause()
    {
        //Debug.Log("Received paused event");
        _isPaused = true;
    }

    void Unpause()
    {
        //Debug.Log("Received unpaused event");
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
