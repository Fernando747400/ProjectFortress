
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
    private enum Anim { Idle, Walk, Death, Hit, Attack, Dance}
    private Anim anim;
    private GameObject DamageTarget;
    private bool IsAttacking;

    public float WallDamage;

    public event Action ZombieDieEvent;
    public event Action <Transform> ZombieTotemEvent;

    //Animations
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        this.speed = UnityEngine.Random.Range(.5f, 4);
        maxDistance = EnemyManagger.Instance.maxDistance;
        SubscribeToPauseEvents();
        anim = Anim.Walk;
        IsAttacking = false;
        GameManager.Instance.FinishGameEvent += LoseAnim;
    }

    void Update()
    {
        animator.SetInteger("Zombie", ((int)anim));
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
        anim = Anim.Walk;
        if (_isPaused) return;
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
        target = transformQueue.Dequeue();
        target = transformQueue.Peek();
    }

    void GetRoute()
    {
        transformQueue = RouteManagger.Instance.RandomRoute();
        target = transformQueue.Peek();
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
            anim = Anim.Death;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall") && !IsAttacking)
        {
            anim = Anim.Attack;
            DamageTarget = collision.gameObject;
            IsAttacking = true;
        }
        if(collision.gameObject.CompareTag("Core") && !IsAttacking)
        {
            DamageTarget.GetComponent<IGeneralTarget>().ReceiveRayCaster(this.gameObject, WallDamage);
            anim = Anim.Death;
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
        anim = Anim.Hit;
        if (_isPaused) return;
        CurrentHp -= dmgValue;
        CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);

    }

    private void LoseAnim()
    {
        anim = Anim.Dance;
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
        anim = Anim.Idle;
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
