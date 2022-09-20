using UnityEngine;

public interface IGeneralTarget
{
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
    //Contains the team that the target is on
    //0 for Red 1 for Blue
    int Team
    {
        get;
        set;
    }
    void TakeDamage(float dmgValue)
    {
        CurrentHp -= dmgValue;
    }
    void Disable(GameObject self)
    {
        if (CurrentHp < 0f)
        {
            self.SetActive(false);
        }   
    }

    void RecieveRayCaster(GameObject sender)
    {
        //needs to evaluate if the sender is on the same team

    }

    
    
}
