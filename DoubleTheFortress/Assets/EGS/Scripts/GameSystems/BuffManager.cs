using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;

    [Header("Sprites")] 
    [SerializeField] private GameObject hammerBuff;
    [SerializeField] private GameObject bulletBuff;
    [SerializeField] private GameObject canonBuff;
    [SerializeField] private GameObject lifeBuff;
    [SerializeField] private GameObject nuke;
    
    [Header("Settings")]
    [SerializeField] private float _buffTime;
    public float CannonDamage;

    [SerializeField] private List<HammerPersistent> _hammerCooldownManager;
    private float _hammerCooldown;

    [SerializeField] private List<Hamer_Grab> _hammerManager;
    private float _hammerUpgradePoints;
    private float _hammerHealthPoints;

    [SerializeField] private List<VrGunFire>_mosquetManager;
    private float _mosquetCooldown;
    private float _mosquetDamage;

    [SerializeField] private List<ButtonCannon> _cannonCooldownManager;
    private float _cannonCooldown;
    private float _currentCannonDamage; //In the bullet

    [SerializeField] private CoreManager _coreManager;
    private float _coreCooldown;

    public event Action<float> AtomicBombEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
        
        hammerBuff.SetActive(false); 
        bulletBuff.SetActive(false); 
        canonBuff.SetActive(false); 
        lifeBuff.SetActive(false); 
        nuke.SetActive(false);
    }

    private void Start()
    {
        SaveAllInitialValues();
    }

    public void CallRandomBuff()
    {
        int SelectedBuff = UnityEngine.Random.Range(1,6); //6 in prod when all buffs are ready 4 for testing
        //int SelectedBuff = 1;
        switch (SelectedBuff)
        {
            case 1:
                DetonateBomb();
                StartCoroutine(AtomicBomb());
            break;

            case 2:
                StartCoroutine(DoBuffAndDebuff(BuffCore, ResetCore));
            break;

            case 3:           
                StartCoroutine(DoBuffAndDebuff(BuffHammer, ResetHammer));
            break;

            case 4:
                StartCoroutine(DoBuffAndDebuff(BuffCannon, ResetCannon));
            break;

            case 5:
                StartCoroutine(DoBuffAndDebuff(BuffMosquet, ResetMosquet));
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
        foreach (HammerPersistent Manager in _hammerCooldownManager)
        {
            Manager.Cooldown = 0f;
        }

        foreach (Hamer_Grab Grab in _hammerManager)
        {
            Grab.PointsToRepair = 10000f;
            Grab.PointsToUpgrade = 10000f;
        }

        hammerBuff.SetActive(true);
    }

    private void ResetHammer()
    {
        foreach (HammerPersistent Manager in _hammerCooldownManager)
        {
            Manager.Cooldown = _hammerCooldown;
        }

        foreach (Hamer_Grab Grab in _hammerManager)
        {
            Grab.PointsToRepair = _hammerHealthPoints;
            Grab.PointsToUpgrade = _hammerUpgradePoints;
        }
        hammerBuff.SetActive(false);
    }

    private void BuffCannon()
    {
        foreach (ButtonCannon cannon in _cannonCooldownManager)
        {
            cannon.Cooldown = 0f;
        }
        CannonDamage = 10000f;
        canonBuff.SetActive(true);
    }

    private void ResetCannon()
    {
        foreach (ButtonCannon cannon in _cannonCooldownManager)
        {
            cannon.Cooldown = _cannonCooldown;
        }
        CannonDamage = _currentCannonDamage;
        canonBuff.SetActive(false);
    }

    private void BuffMosquet()
    {
        foreach (VrGunFire gun in _mosquetManager)
        {
            gun.CoolDown = 0f;
            gun.Damage = 10000f;
        }
        bulletBuff.SetActive(true);
    }

    private void ResetMosquet()
    {
        foreach (VrGunFire gun in _mosquetManager)
        {
            gun.CoolDown = _mosquetCooldown;
            gun.Damage = _mosquetDamage;
        }
        bulletBuff.SetActive(false);
    }

    private void BuffCore()
    {
        lifeBuff.SetActive(true);
        _coreManager.Cooldown = 30f;
    }

    private void ResetCore()
    {
        _coreManager.Cooldown = _coreCooldown;
        lifeBuff.SetActive(false);
    }

    private void DetonateBomb()
    {
        AtomicBombEvent?.Invoke(1000000000000000);
    }

    private void SaveAllInitialValues()
    {
        _hammerCooldown = _hammerCooldownManager[0].Cooldown;
        _hammerHealthPoints = _hammerManager[0].PointsToRepair;
        _hammerUpgradePoints = _hammerManager[0].PointsToUpgrade;

        _mosquetCooldown = _mosquetManager[0].CoolDown;
        _mosquetDamage = _mosquetManager[0].Damage;

        _cannonCooldown = _cannonCooldownManager[0].Cooldown;
        _currentCannonDamage = CannonDamage;

        _coreCooldown = _coreManager.Cooldown;
    }

    private IEnumerator AtomicBomb()
    {
        nuke.SetActive(true);
        yield return new WaitForSeconds(5);
        nuke.SetActive(false);
    }
}
