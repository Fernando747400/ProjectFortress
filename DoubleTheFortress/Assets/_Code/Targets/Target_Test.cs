using UnityEngine;

public class Target_Test : MonoBehaviour, IGeneralTarget
{
    public bool Sensitive { get; set; }
    public float CurrentHp { get; set; }
    public float MaxHp { get; set; }


    // Start is called before the first frame update
    private void Start()
    {
        CurrentHp = MaxHp;
    }
    
    

}
