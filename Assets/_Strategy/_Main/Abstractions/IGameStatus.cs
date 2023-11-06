using System;


namespace _Strategy._Main.Abstractions
{
    
    public interface IGameStatus
    {
        
        IObservable<int> Status { get; }
        
    }
}