using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePursuit : StearingBehaviours, IGeneralTarget, IPause
{
    public enum ZombieState { Idle, Walk, Death, Hit, Attack, Dance, Start }
    public ZombieState CurrentState;

    private float _arrivalDistance;
    private float _zombieDamage;

    private Queue<Transform> _routeQueue = new Queue<Transform>();
    private Transform _targetTransform;
    private float _distanceToTarget;

    private GameObject _targetToDamage;
    private bool _isAttacking;
    private bool _isRecivingDamage;

    private ZombieAnimator _zombieAnimator;
    public float MaxDistance { get => _arrivalDistance; set => _arrivalDistance = value; }
    public float ZombieDamage { get => _zombieDamage; set => _zombieDamage = value; }

    public event Action ZombieDieEvent;
    public event Action <Transform> ZombieTotemEvent;


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
                break;     
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Final"))
        {
            CurrentState = ZombieState.Death;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (CurrentState != ZombieState.Walk) return;
        if (collision.gameObject.CompareTag("Wall") && !_isAttacking)
        {
            _targetToDamage = collision.gameObject;
            CurrentState = ZombieState.Attack;
        }
        if (collision.gameObject.CompareTag("Core"))
        {
            _targetToDamage = collision.gameObject;
            DoDamageToTarget();
            Despawn();
        }
    }

    private void Idle()
    {
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Idle);
    }

    private void Pursuit()
    {
        if (_isPaused) return;
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Walk);
        _distanceToTarget = Vector3.Distance(this.transform.position, _targetTransform.position);
        if (_distanceToTarget < _arrivalDistance)
        {
            UpdateTarget();
        }
        Vector3 pursuit = this.Pursuit(_targetTransform.position);
        transform.LookAt(_targetTransform);
        transform.position += pursuit * Time.deltaTime;
    }

    private void UpdateTarget()
    {
        if(_routeQueue.Count == 0)
        {
            CurrentState = ZombieState.Idle;
            return;
        }
        _routeQueue.Dequeue();
        _targetTransform = _routeQueue.Peek();
    } 

    private void Die()
    {
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Death); //Anim should call Despawn
        _routeQueue.Clear();
        _targetTransform = null;
        ZombieDieEvent?.Invoke();
        ZombieTotemEvent?.Invoke(this.transform);
    }

    public void Despawn()
    {
        EnemyManagger.Instance.Despawn(EnemyManagger.Instance.Zombie, this.gameObject);
        EnemyManagger.Instance.OnSpawn();
    }

    private void Attack(GameObject target)
    {
        if (_isPaused) return;
        if (_isAttacking) return;
        _isAttacking = true;
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Attack);
    }

    public void DoDamageToTarget() //This is called by Zombie Attack anim
    {
        _targetToDamage.GetComponent<IGeneralTarget>().ReceiveRayCaster(this.gameObject, _zombieDamage);
    }

    public void FinishAttack() //This is called by Zombie Attack anim
    {
        _isAttacking = false;
        CurrentState = ZombieState.Walk;
    }

    protected virtual void TakeDamage(float dmgValue)
    {
        if (_isRecivingDamage) return;
        _isRecivingDamage = true;
        CurrentState = ZombieState.Hit;
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Hit);
        ReceiveDamage(dmgValue);
    }

    private void ReceiveDamage(float damage)
    {
        if (_isPaused) return;
        CurrentHp -= damage;
        CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);

        if (CurrentHp <= 0) CurrentState = ZombieState.Death;
    }

    public void FinishDamage() //This should be called by animator
    {
        _isRecivingDamage = false;
        CurrentState = ZombieState.Walk;
    }

    private void GameOverAnim()
    {
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Dance);
    }
    private void GetRoute()
    {
        _routeQueue = RouteManagger.Instance.RandomRoute();
        _targetTransform = _routeQueue.Peek();
    }

    private void Prepare()
    {
        GetRoute();
        _zombieAnimator = GetComponent<ZombieAnimator>();
        _arrivalDistance = EnemyManagger.Instance.maxDistance;
        this.speed = UnityEngine.Random.Range(.5f, 4);

        _isAttacking = false;
        _isRecivingDamage = false;

        CurrentState = ZombieState.Idle;
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
    }

    private void UnsubscribeToEvents()
    {
        GameManager.Instance.PauseGameEvent -= Pause;
        GameManager.Instance.PlayGameEvent -= Unpause;
        GameManager.Instance.FinishGameEvent -= GameOverAnim;
        ZombieDieEvent -= GameManager.Instance.AddKill;
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
