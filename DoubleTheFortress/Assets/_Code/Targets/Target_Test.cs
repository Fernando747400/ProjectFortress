using UnityEngine;

public class Target_Test : MonoBehaviour ,IGeneralTarget
{
    public bool Sensitive
    {
        get { return _sensitive; }
        set { _sensitive = value; }
    }
    public float MaxHp
    {
        get { return _maxHp; }
        set { _maxHp = value; }
    }
    public float CurrentHp
    {
        get { return _currentHp; }
        set { _currentHp = value; }
    }

    private bool _sensitive;
    private float _currentHp;
    private float _maxHp;
    
    // Start is called before the first frame update
    private void Start()
    {
        CurrentHp = MaxHp;
        Sensitive = true;
    }
}
