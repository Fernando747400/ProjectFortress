using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;

    [Header("Settings")]
    [SerializeField] private float _buffTime;
    public float CannonDamage;

    [SerializeField] private HammerPersistent _hammerCooldownManager;
    private float _hammerCooldown;

    [SerializeField] private Hamer_Grab _hammerManager;
    private float _hammerUpgradePoints;
    private float _hammerHealthPoints;

    [SerializeField] private VrGunFire _mosquetManager;
    private float _mosquetCooldown;
    private float _mosquetDamage;

    [SerializeField] private ButtonCannon _cannonCooldownManager;
    private float _cannonCooldown;
    private float _currentCannonDamage; //In the bullet

    [SerializeField] private CoreManager _coreManager;
    private float _coreCooldown;

    public event Action AtomicBombEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    public void CallRandomBuff()
    {
        int SelectedBuff = UnityEngine.Random.Range(1,6);

        switch (SelectedBuff)
        {
            case 1:
                StartCoroutine(DoBuffAndDebuff(BuffHammer, ResetHammer));
            break;

            case 2:
                StartCoroutine(DoBuffAndDebuff(BuffCannon, ResetCannon));
            break;

            case 3:
                StartCoroutine(DoBuffAndDebuff(BuffMosquet, ResetMosquet));
            break;

            case 4:
                StartCoroutine(DoBuffAndDebuff(BuffCore, ResetCore));
            break;

            case 5:
                DetonateBomb();
            break;
        }
    }

    private IEnumerator DoBuffAndDebuff(Action bufferFunc, Action debufferFunc)
    {
        bufferFunc.Invoke();
        yield return new WaitForSeconds(_buffTime);
        debufferFunc.Invoke();
    }

    private void BuffHammer()
    {
        _hammerCooldownManager.Cooldown = 0f;
        _hammerManager.PointsToRepair = 10000f;
        _hammerManager.PointsToUpgrade = 10000f;
    }

    private void ResetHammer()
    {
        _hammerCooldownManager.Cooldown = _hammerCooldown;
        _hammerManager.PointsToRepair = _hammerHealthPoints;
        _hammerManager.PointsToUpgrade = _hammerUpgradePoints;
    }

    private void BuffCannon()
    {
        //_cannonCooldownManager.Cooldwon = 0f;
        CannonDamage = 10000f;
    }

    private void ResetCannon()
    {
        //_cannonCooldownManager.Cooldown = _cannonCooldown;
        CannonDamage = _currentCannonDamage;
    }

    private void BuffMosquet()
    {
        //_mosquetManager.Cooldown = 0f;
        //_mosquetManager.Damage = 10000f;
    }

    private void ResetMosquet()
    {
        //_mosquetManager.Cooldwon = _mosquetCooldown;
        //_mosquetManager.Cooldown = _mosquetDamage;
    }

    private void BuffCore()
    {
        _coreManager.Cooldown = 30f;
    }

    private void ResetCore()
    {
        _coreManager.Cooldown = _coreCooldown;
    }

    private void DetonateBomb()
    {
        AtomicBombEvent?.Invoke();
    }
}
