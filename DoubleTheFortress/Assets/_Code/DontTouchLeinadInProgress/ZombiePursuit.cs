using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePursuit : StearingBehaviours, IGeneralTarget, IPause
{
    [Header("Pursuit Dependences")]
    private Queue<Transform> transformQueue = new Queue<Transform>();
    private Transform target;
    private float distance;
    public float maxDistance;

    private GameObject DamageTarget;
    private bool IsAttacking;

    public float WallDamage;

    public event Action ZombieDieEvent;
    public event Action <Transform> ZombieTotemEvent;

    //Animations
    private ZombieAnimator _zombieAnimator;
    private void Start()
    {
        _zombieAnimator = GetComponent<ZombieAnimator>();
        this.speed = UnityEngine.Random.Range(.5f, 4);
        maxDistance = EnemyManagger.Instance.maxDistance;
        SubscribeToPauseEvents();
        IsAttacking = false;
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Idle);
        GameManager.Instance.FinishGameEvent += LoseAnim;
    }

    void Update()
    {
        try 
        {
           Pursuit();
        }
        catch
        {
           GetRoute();
        }   
    }

    void Pursuit()
    {
        if (_isPaused) return;
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Walk);
        distance = Vector3.Distance(this.transform.position, target.transform.position);
        if (distance < maxDistance)
        {
            UpdateTarget();
        }
        Vector3 pursuit = this.Pursuit(target.transform.position);
        transform.LookAt(target);
        transform.position += pursuit * Time.deltaTime;
    }

    private void UpdateTarget()
    {
        transformQueue.Dequeue();
        target = transformQueue.Peek();
    }

    void GetRoute()
    {
        transformQueue = RouteManagger.Instance.RandomRoute();
        target = transformQueue.Peek();
    }

    public void Die()
    {
        transformQueue.Clear();
        target = null;
        ZombieDieEvent?.Invoke();
        ZombieTotemEvent?.Invoke(this.transform);
        EnemyManagger.Instance.Despawn(EnemyManagger.Instance.Zombie, this.gameObject);
        EnemyManagger.Instance.OnSpawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Final"))
        {
            _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Death);
            this.speed = 0f;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") && !IsAttacking)
        {
            _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Attack);
            DamageTarget = collision.gameObject;
            IsAttacking = true;
        }
        if(collision.gameObject.CompareTag("Core") && !IsAttacking)
        {
            DamageTarget.GetComponent<IGeneralTarget>().ReceiveRayCaster(this.gameObject, WallDamage);
            _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Death);
        }
    }

    public void DoDamageToTarget()
    {
        DamageTarget.GetComponent<IGeneralTarget>().ReceiveRayCaster(this.gameObject, WallDamage);
    }

    public void FinishAtack()
    {
        IsAttacking = false;  
    }

    protected virtual void TakeDamage(float dmgValue)
    {
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Attack);
        if (_isPaused) return;
        CurrentHp -= dmgValue;
        CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);

    }

    private void LoseAnim()
    {
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Dance);
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
        _zombieAnimator.PlayAnimation(ZombieAnimator.AnimationsEnum.Idle);
        _isPaused = true;
    }

    void Unpause()
    {
        _isPaused = false;
    }

    private void SubscribeToPauseEvents()
    {
        GameManager.Instance.PauseGameEvent += Pause;
        GameManager.Instance.PlayGameEvent += Unpause;
    }
    #endregion
}
