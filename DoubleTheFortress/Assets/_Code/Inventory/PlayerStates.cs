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
        None = -1,  
        Hammer = 0 ,
        Musket = 1,
        Defibrillator = 2,
        Torch = 3,

        
        
        // Selecting = 16,
        // ObjectsRightHand = Musket | Hammer,
        // ObjectsLeftHand =  Torch | Defibrillator,
    }
    
}