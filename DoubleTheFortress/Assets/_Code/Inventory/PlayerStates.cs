namespace DebugStuff.Inventory
{
    public enum PlayerStates
    {
        Idle,
        Interacting
    }

    public enum PlayerInteractions
    {
        WallInteraction,
        WeaponInteraction,
        GrabbingInteraction,
    }
    
    [System.Flags]
    public enum PlayerSelectedItem
    {
        None = 0,  
        Hammer = 1 ,
        Musket =2,
        Defibrillator = 4,
        Torch = 8,
        Selecting = 16,

        
        
        ObjectsRightHand = Musket | Hammer,
        ObjectsLeftHand =  Torch | Defibrillator,
    }
    
}