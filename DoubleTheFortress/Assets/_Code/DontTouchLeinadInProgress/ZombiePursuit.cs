
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

    public event Action ZombieDieEvent;
    public event Action <Transform> ZombieTotemEvent;

    //Animations
    private bool _walk = true;
    private bool _idle = false;
    private bool _dance = false;
    private bool _die = false;
    private bool _damage = false;

    private void Start()
    {
        this.speed = UnityEngine.Random.Range(.5f, 4);
        maxDistance = EnemyManagger.Instance.maxDistance;
        SubscribeToPauseEvents();
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

    void Die()
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
            Die();
        }
    }

    protected virtual void TakeDamage(float dmgValue)
    {
        if (_isPaused) return;
        CurrentHp -= dmgValue;
        CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);
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
        _isPaused = true;
        _walk = false;
        _idle = true;
        _dance = false;
        _die = false;
        _damage = false;
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
