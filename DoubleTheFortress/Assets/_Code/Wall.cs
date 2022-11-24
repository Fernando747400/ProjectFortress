using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Wall : MonoBehaviour, IConstructable, IGeneralTarget
{
    [Header("Settings")]
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _upgradePoints;

    [Header("Events")]
    public UnityEvent BuildEvent;
    public UnityEvent RepairEvent;
    public UnityEvent UpgradePointsEvent;
    public UnityEvent UpgradeEvent;
    public UnityEvent ReceiveHammerEvent;
    public UnityEvent ReceiveDamageEvent;

    private GameObject _currentObject;
    private float _maxHealth;
    private float _upgradePointsRequired;
    private bool _sensitive;
    private float _maxHp;
    private float _currentHp;

    public delegate void OnRecieveHammer(float repairValue, float upgradeValue);
    public event OnRecieveHammer onRecieveHammer;

    public delegate void OnRecieveDamage(GameObject Sender, float damage);
    public event OnRecieveDamage onRecieveDamage;

    #region Get and set
    public float CurrentHealth { get => _currentHealth; set { _currentHealth = value; _currentHp = value; } }
    public float MaxHealth { get => _maxHealth; set { _maxHealth = value;  _maxHp = value; } }
    public float MaxHp { get => _maxHp; set { _maxHp = value; _maxHealth = value; } }
    public float CurrentHp { get => _currentHp; set { _currentHp = value; _currentHealth = value; } }
    public float UpgradePoints { get => _upgradePoints; set => _upgradePoints = value; }
    public float UpgradePointsRequired { get => _upgradePointsRequired; set => _upgradePointsRequired = value; }
    public bool Sensitive { get => _sensitive; set => _sensitive = value; }
    public GameObject CurrentObject { get => _currentObject; set => _currentObject = value; }

    public UnityEvent IBuildEvent => BuildEvent;
    public UnityEvent IRepairEvent => RepairEvent;
    public UnityEvent IUpgradePointsEvent => UpgradePointsEvent;
    public UnityEvent IUpgradeEvent => UpgradeEvent;
    public UnityEvent IRecieveHammerEvent => IRecieveHammerEvent;

    #endregion

    private void Awake()
    {
        _maxHp = _maxHealth;
        _currentHp = _currentHealth;
    }

    public void RecieveHammer(float repairValue, float upgradeValue)
    {
        onRecieveHammer?.Invoke(repairValue, upgradeValue);
        ReceiveHammerEvent?.Invoke();
    }

    protected virtual void TakeDamage(float dmgValue)
    {
        CurrentHealth -= dmgValue;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);
        Debug.Log("TookDamage");
    }

    void CheckHp (GameObject self)
    {
        Debug.Log("Not necesary in wall");
    }

    public void ReceiveRayCaster(GameObject sender, float dmg)
    {
        //needs to evaluate if the sender is on the same team
        onRecieveDamage?.Invoke(sender, dmg);
        ReceiveDamageEvent?.Invoke();
        Debug.Log("Hit By: " + sender.gameObject.name);
    }

}
