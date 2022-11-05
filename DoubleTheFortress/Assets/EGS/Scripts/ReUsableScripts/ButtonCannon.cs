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

   [SerializeField] [Range(0, 1f)] private float _maxValue = 0.5f;
   
   private bool _isPaused;
    
    float _time;
    float _intialTimer = 0;
    private float _initialValue;
    private bool _timerHasStarted;
    private bool _timerHasFinished;
    private bool _isFiring;

    public Action OnPushedButton;

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
        yield return new WaitForSeconds(1f);
        OnPushedButton?.Invoke();
    }
    void FireCannon()
    {
        particles.SetActive(false);
        cannon.Launch();
        _isFiring = false;
        if (!_isMenuCannon)
        {
            print("Cañon con cool down");
            StartTimer();
        }
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
    }

    void HandleTimer()
    {
        if (!_timerHasStarted && _timerHasFinished) return;
        
        _time += Time.deltaTime * 1;
        LerpColor();
       // Debug.Log(_time);
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
        
        // if(_isAutomatic) StartTimer();
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
