using UnityEngine;

public class GeneralAgressor : MonoBehaviour
{

    [SerializeField] protected float damage;
    protected bool TryGetGeneralTarget(GameObject target)
    {
        try
        { 
            print(target.name);
            bool result = target.GetComponent<IGeneralTarget>().Sensitive; 
            return result;
        }
        catch
        {
            return false;
        }
    }
}