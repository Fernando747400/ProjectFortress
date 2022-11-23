using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCannon : MonoBehaviour
{
    [Header("Cannon config")]
    [SerializeField] private GameObject particles;
    [SerializeField] private Debug_CannonFire cannon;

    [SerializeField] private Collider _collider;
    [SerializeField] private float delay;
    [SerializeField] private bool _isMenuCannon;

    [Header("Cannon UI")] 
    
    [SerializeField] private Image fill;
    [SerializeField] private Image background;

   [SerializeField] [Range(0, 1f)] private float _maxValue = 0.5f;
   
   [Header("AudioClips")]
    public AudioClip _fireSound;
   
   private bool _isPaused;
   public bool _isAutomatic;

   [SerializeField]private float _cooldown;

   float _time;
    float _intialTimer = 0;
    private float _initialValue;
    private bool _timerHasStarted;
    private bool _timerHasFinished;
    private bool _isFiring;

    public Action OnPushedButton;

    public float Cooldown
    {
        get { return _cooldown;}
        set { _cooldown = value; }
    }

    //LerpColor
    [SerializeField][Range(0f, 1f)] float lerpTime;
    int colorIndex = 0;
    float t = 0;
    int len;
    public float LerpChange;
    [SerializeField] Color[] myColors;


    void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        OnPushedButton += FireCannon;
        particles.SetActive(false);
        len = myColors.Length;
        _timerHasStarted = false;
        _timerHasFinished = true;
        fill.color = myColors[2];
        // StartTimer();
    }

    void Update()
    { 
        HandleTimer();
        
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGameEvent += PauseGame;
            GameManager.Instance.PlayGameEvent += UnPauseGame;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGameEvent -= PauseGame;
            GameManager.Instance.PlayGameEvent -= UnPauseGame;
        }
    }

    void PauseGame()
    {
        _isPaused = true;
    }
    void UnPauseGame()
    {
        _isPaused = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Torch"))
        {
            if (_isPaused)
            {
                return;
            }
            
            if (!_isFiring && _timerHasFinished)
            {
                StartCoroutine(FireTimer());
            }
            
        }
    }

    IEnumerator FireTimer()
    {
        _isFiring = true;
        particles.SetActive(true);
        yield return new WaitForSeconds(_cooldown);
        OnPushedButton?.Invoke();
    }
    void FireCannon()
    {
        particles.SetActive(false);
        cannon.Launch();
        PlayAudio(_fireSound,0.5f);
        _isFiring = false;
        if (!_isMenuCannon)
        {
            StartTimer();
        }
    }

    private void PlayAudio(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayAudio(clip, volume, this.transform.position);
    }
    void HandleUICannon(float value)
    {
       _initialValue = Mathf.Lerp(0, _maxValue, value);
       fill.fillAmount = _initialValue;
    }

    void StartTimer()
    {
        // Debug.Log("Start timer");
        _time = _intialTimer;
        _timerHasStarted = true;
        _timerHasFinished = false;
        colorIndex = 0;
        fill.color = myColors[0];
        background.color = new Color(0, 0, 0, 0);

    }

    void HandleTimer()
    {
        if (!_timerHasStarted && _timerHasFinished) return;
        
        _time += Time.deltaTime * 1;
        
        LerpColor();
        if (_time > delay)
        {
            RestartTimer();
        }

        HandleUICannon(_time / delay);
    }

    void RestartTimer()
    {
        _time = 0;
        _timerHasFinished = true;
        _timerHasStarted = false;
        background.color = new Color(0, 0, 0, 1);
        background.color = myColors[2];
        if(_isAutomatic) StartTimer();
    }
    
    void LerpColor()
    {
        fill.color = Color.Lerp(fill.color, myColors[colorIndex], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, LerpChange, lerpTime * Time.deltaTime);
        if(t>.9f)
        {
            t = 0;
            colorIndex++;
            colorIndex = (colorIndex >= len) ? 0 : colorIndex;
        }
    }
}
