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
    public enum PlayerSelectedItem
    {
        None,
        Hammer,
        Musket,
        Defibrillator,
        Selecting,
    }
    
}