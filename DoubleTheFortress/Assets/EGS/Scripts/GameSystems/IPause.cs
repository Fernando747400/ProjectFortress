public interface IPause 
{
   bool IsPaused { set; }
    
    void Pause()
    {
        IsPaused = true;
    }

    void Unpause()
    {
        IsPaused = false;
    }
}
