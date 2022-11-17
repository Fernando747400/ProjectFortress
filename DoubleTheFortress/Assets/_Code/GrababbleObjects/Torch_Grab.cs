using UnityEngine;

public class Torch_Grab : IGrabbable
{
    [SerializeField] private GameObject _particles;
    void Start()
    {
        // _particles.SetActive(false);
    }
    
    public override void HandleSelectedState(bool isSelecting)
    {
        // base.HandleSelectedState(isSelecting);
        //
        // if (!isSelecting)
        // {
        //     _particles.SetActive(true);
        // }
        // else
        // {
        //     _particles.SetActive(false);
        // }
    }
}
