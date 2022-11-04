using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePursuit : StearingBehaviours, IGeneralTarget, IPause
{   
    public enum ZombieState { Idle, Walk, Death, Hit, Attack, Dance, Start }
    public ZombieState CurrentState;

    [Header("Dependencies")]
    [SerializeField] private GameObject _alertSignal;

    private float _arrivalDistance;
    private float _zombieDamage;

    private Queue<Transform> _routeQueue = new Queue<Transform>();
    private Transform _targetTransform;
    private float _distanceToTarget;

    private GameObject _targetToDamage;
    private bool _isAttacking;
    private bool _isRecivingDamage;

    private ZombieAnimator _zombieAnimator;
    private Rigidbody _rigidBody;
    public float MaxDistance { get => _arrivalDistance; set => _arrivalDistance = value; }
    public float ZombieDamage { get => _zombieDamage; set => _zombieDamage = value; }
    public Queue<Transform> RouteQueue { get => _routeQueue; set => _routeQueue = value; }

    public event Action ZombieDieEvent;
    public event Action <Transform> ZombieTotemEvent;
    private bool isAvoid = false;

    public float timeOfAvoid = 2;

    private void Start()
    {
        Prepare();
    }

    void Update()
    {
        DoStateMachine();
    }

    private void DoStateMachine()
    {
        switch (CurrentState)
        {
            case ZombieState.Idle:
                Idle();
            break;

            case ZombieState.Walk:
                Pursuit();
            break;

            case ZombieState.Death:
                Die();
                break;

            case ZombieState.Hit:
                break;

            case ZombieState.Attack:
                Attack(_targetToDamage);
                break;

            case ZombieState.Dance:
                GameOverAnim();
                break;     
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Core"))
        {
            _targetToDamage = other.gameObject;
            DoDamageToTarget();
            Despawn();
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            this.avoidGameObject = collision.gameObject;
            isAvoid = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            StartCoroutine(AvoidColdown());
        }
    }

    IEnumerator AvoidColdown()
    {
        yield return new WaitForSeconds(timeOfAvoid);
        isAvoid = false;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (CurrentState != ZombieState.Walk) return;
        if (collision.gameObject.CompareTag("Wall") && !_isAttacking)
        {
            _targetToDamage = collision.gameObject;
            CurrentState = ZombieState.Attack;
        }
    }

    private void Idle()
    {
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Idle);
        _rigidBody.velocity = Vector3.zero;
    }

    private void Pursuit()
    {
        if (_isPaused) return;
        if (_targetTransform == null) GetRoute();
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Walk);
        _distanceToTarget = Vector3.Distance(this.transform.position, _targetTransform.position);
        if (_distanceToTarget < _arrivalDistance)
        {
            UpdateTarget();
        }
        Vector3 pursuit = this.Pursuit(_targetTransform.position);
        Vector3 stearing = pursuit;
        if (isAvoid)
        {
            Vector3 avoid = this.Avoid();
            stearing += avoid;
        }
        
        //transform.LookAt(_targetTransform);
        iTween.LookUpdate(this.transform.gameObject, iTween.Hash("looktarget", _targetTransform,"axis", "y", "time", 2f));
        transform.position += stearing * Time.deltaTime;
    }

    private void UpdateTarget()
    {
        _routeQueue.Dequeue();
        if(_routeQueue.Count == 0)
        {
            CurrentState = ZombieState.Idle;
            return;
        }
        _targetTransform = _routeQueue.Peek();
    } 

    private void Die()
    {
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Death); //Anim should call Despawn
        ZombieDieEvent?.Invoke();
        ZombieTotemEvent?.Invoke(this.transform);
    }

    public void Despawn()
    {
        _routeQueue.Clear();
        _targetTransform = null;
        EnemyManagger.Instance.Despawn(EnemyManagger.Instance.Zombie, this.gameObject);
        EnemyManagger.Instance.OnSpawn();
    }

    private void Attack(GameObject target)
    {
        if (_isPaused) return;
        if (_isAttacking) return;
        _isAttacking = true;
        _alertSignal.SetActive(true);
        _rigidBody.velocity = Vector3.zero;
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Attack);
        iTween.LookUpdate(this.transform.gameObject, iTween.Hash("looktarget", target.transform, "axis", "y", "time", 1f));
    }

    public void DoDamageToTarget() //This is called by Zombie Attack anim
    {
        if (_isPaused) return;
        _targetToDamage.GetComponent<IGeneralTarget>().ReceiveRayCaster(this.gameObject, _zombieDamage);
    }

    public void FinishAttack() //This is called by Zombie Attack anim
    {
        if (_isPaused) return;
        _isAttacking = false;
        _alertSignal.SetActive(false);
        CurrentState = ZombieState.Walk;
    }

    protected virtual void TakeDamage(float dmgValue)
    {
        if (_isPaused) return;
        if (_isRecivingDamage) return;
        _isAttacking = false;
        _alertSignal.SetActive(false);
        _isRecivingDamage = true;
        CurrentState = ZombieState.Hit;
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Hit);
        ReceiveDamage(dmgValue);
    }

    private void ReceiveDamage(float damage)
    {
        if (_isPaused) return;
        _life -= damage;
        _life = Mathf.Clamp(_life, 0, _maxLife);

        if (_life <= 0) CurrentState = ZombieState.Death;
    }

    public void FinishDamage() //This should be called by animator
    {
        _isRecivingDamage = false;
        CurrentState = ZombieState.Walk;
    }

    private void GameOverAnim()
    {
        CurrentState = ZombieState.Dance;
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Dance);
        _rigidBody.velocity = Vector3.zero;
    }
    private void GetRoute()
    {
        _routeQueue = RouteManagger.Instance.RandomRoute();
        _targetTransform = _routeQueue.Peek();
        ResetZombie();
    }
    
    public void ResetZombie()
    {
        if (CurrentState == ZombieState.Idle) CurrentState = ZombieState.Walk;
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Walk);
        CheckIfPause();
        StrongZombie();
        _alertSignal.SetActive(false);
        //_maxLife = EnemyManagger.Instance.ZombieLife;
        _life = _maxLife;
        _isSensitive = true;
    }

    private void Prepare()
    {
        _zombieAnimator = GetComponent<ZombieAnimator>();
        _rigidBody = GetComponent<Rigidbody>();
        GetRoute();
        _arrivalDistance = EnemyManagger.Instance.maxDistance;
        this.speed = UnityEngine.Random.Range(.2f, .4f);

        _isAttacking = false;
        _isRecivingDamage = false;

        CurrentState = ZombieState.Walk;
        StrongZombie();
        CheckIfPause();
        _alertSignal.SetActive(false);
    } 

    public void CheckIfPause()
    {
        if(GameManager.Instance.IsPaused) CurrentState = ZombieState.Idle;
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Idle);
    }

    private void StrongZombie()
    {
        if (EnemyManagger.Instance.StrongZombie)
        {
            foreach (Renderer render in this.GetComponentsInChildren<Renderer>())
            {
                if (render.material.name == "Zombieskin (Instance)") render.material = EnemyManagger.Instance.StrongSkin;
            }
        }
        if (!EnemyManagger.Instance.StrongZombie)
        {
            foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
            {
                if (renderer.material.name == "DefaultSkin (Instance)") renderer.material = EnemyManagger.Instance.DefaultSkin;
            }
        }
    }

    #region Interface Methods

    private float _maxLife;
    private float _life;
    private bool _isPaused;
    private bool _isSensitive;
    public bool IsPaused { set => _isPaused = value; }
    public bool Sensitive { get => _isSensitive; set => _isSensitive = value; }
    public float MaxHp { get => _maxLife; set => _maxLife = value; }
    public float CurrentHp { get => _life; set => _life = value; }
    
    void Pause()
    {
        CurrentState = ZombieState.Idle;
        _isPaused = true;
    }

    void Unpause()
    {
        CurrentState = ZombieState.Walk;
        _isPaused = false;
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.PauseGameEvent += Pause;
        GameManager.Instance.PlayGameEvent += Unpause;
        GameManager.Instance.FinishGameEvent += GameOverAnim;
        ZombieDieEvent += GameManager.Instance.AddKill;
        ZombieTotemEvent += UIManager.Instance.ZombieDeadEffect;
    }

    private void UnsubscribeToEvents()
    {
        GameManager.Instance.PauseGameEvent -= Pause;
        GameManager.Instance.PlayGameEvent -= Unpause;
        GameManager.Instance.FinishGameEvent -= GameOverAnim;
        ZombieDieEvent -= GameManager.Instance.AddKill;
        ZombieTotemEvent -= UIManager.Instance.ZombieDeadEffect;
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
