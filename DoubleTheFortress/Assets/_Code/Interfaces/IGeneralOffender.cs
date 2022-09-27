using UnityEngine;

public class IGeneralOffender : MonoBehaviour
{

    protected float Damage
    {
        get;
        set;
    }
    protected bool TryGetGeneralTarget(GameObject target)
    {
        try
        { 
            bool result = target.GetComponent<IGeneralTarget>().Sensitive; 
            return result;
        }
        catch
        {
            return false;
        }
    }
}