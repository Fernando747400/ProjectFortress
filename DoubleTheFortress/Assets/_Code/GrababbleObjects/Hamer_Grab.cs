using System;
using TMPro;
using UnityEngine;

public class Hamer_Grab : IGrabbable , IPause
{

    #region Variables
    [Header("Dependencies")]
    [SerializeField] private HammerPersistent _persistentData;

    [Header("UI")] 
    [SerializeField] private GameObject _uiHammer;

    [Header("Settings")]
    [SerializeField] private float _pointsToRepair;
    [SerializeField] private float _pointsToUpgrade;

    [Header("AudioClips")]
    public AudioClip _wooodSound;
    public AudioClip _metalSound;
    public AudioClip _brickSound;

    public event Action<GameObject> ConstructableHitEvent;
    public event Action DisableHammerEvent;
    

    #endregion

    #region unity Methods
    void Start()
    {
        _isPaused = GameManager.Instance.IsPaused;
        if (_persistentData == null) throw new NullReferenceException("No persitent data script is set into the Hammer");
    }
   
    #endregion
   

    #region public Methods
    
    
    #endregion

    #region private Methods

    private void OnTriggerEnter(Collider other)
    {
        if (_isPaused) return;
        if (other.gameObject.GetComponent<IConstructable>() != null && _persistentData.CooldownPassed())
        {
            _persistentData.ResetCooldwon();
            other.GetComponent<IConstructable>().RecieveHammer(_pointsToRepair, _pointsToUpgrade);

            WallManager wallManager = other.GetComponent<WallManager>();

            if (wallManager != null)
            {            
                PlayAudio(wallManager, 0.5f );
            }

            ConstructableHitEvent?.Invoke(other.gameObject);
        }
    }

  
    public override void HandleSelectedState(bool isSelecting)
    {
        base.HandleSelectedState(isSelecting);

        if (isSelecting)
        {
            HammerStatusBars hammerStatusBars = GetComponent<HammerStatusBars>();
            hammerStatusBars.ResetCanvasBars();
        }
    }
    #endregion

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

    private void PlayAudio( WallManager wall ,float volume = 1f)
    {

        AudioClip clip = null;
        
        switch (wall.WallIndex)
        {
            case 0:
                clip = null;
                break;
            case 1:
                clip = _wooodSound;
                break;
            case 2:
                clip = _metalSound;
                break;
            case 3:
                clip = _brickSound;
                break;
        }
        if (clip == null) return;    
        if (AudioManager.Instance != null) AudioManager.Instance.PlayAudio(clip, volume, this.transform.position);
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
        if (GameManager.Instance != null)
        {
            SubscribeToEvents();
            _isPaused = GameManager.Instance.IsPaused;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            UnsubscribeToEvents();
        }
    }
    #endregion

}
