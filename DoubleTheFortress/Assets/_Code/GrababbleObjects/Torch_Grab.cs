using UnityEngine;

public class Torch_Grab : IGrabbable
{
    // [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private GameObject _particles;
    void Start()
    {
        // _inventoryController.OnIsSelecting += HandleSelectingState;
        _particles.SetActive(false);
    }
    
    // void HandleSelectingState(bool isSelecting)
    // {
    //     if (isSelecting)
    //     {
    //         _collider.enabled = false;
    //     }
    //     else
    //     {
    //         _particles.SetActive(true);
    //         _collider.enabled = true;
    //     }
    //     
    // }

    public override void HandleSelectedState(bool isSelecting)
    {
        base.HandleSelectedState(isSelecting);
        
        if (!isSelecting)
        {
            _particles.SetActive(true);
        }
        else
        {
            _particles.SetActive(false);
        }
    }
}
