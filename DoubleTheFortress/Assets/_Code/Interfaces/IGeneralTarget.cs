using UnityEngine;

public interface IGeneralTarget
{
    bool Sensitive
    {
        get;
        set;
    }
    float MaxHp
    {
        get;
        set;
    }    
    float CurrentHp
    {
        get;
        set;
    }
    void TakeDamage(float dmgValue)
    {
        CurrentHp -= dmgValue;
    }
    
    void CheckHp(GameObject self)
    {
        if (CurrentHp < 0f)
        {
            self.SetActive(false);
        }   
    }
    
    public void ReceiveRayCaster(GameObject sender, float dmg)
    {
        //needs to evaluate if the sender is on the same team
        Debug.Log("Hit By: " + sender.gameObject.name);
        TakeDamage(dmg);

    }

    
    
}
