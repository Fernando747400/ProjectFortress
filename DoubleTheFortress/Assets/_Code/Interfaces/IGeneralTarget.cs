using UnityEngine;

public interface IGeneralTarget
{
    float GTargetMaxHp
    {
        get;
        set;
    }    
    float GTargetCurrentHp
    {
        get;
        set;
    }

    void TakeDamage(float dmgValue)
    {
        GTargetCurrentHp -= dmgValue;
    }

    void Disable()
    {
        if (GTargetCurrentHp < 0f)
        {
            
        }
        
    }

    
    
}
