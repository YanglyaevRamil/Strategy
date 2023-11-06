using System;


namespace _Strategy._Main.Abstractions
{
    
    public interface ITimeModel
    {
        
        IObservable<int> GameTime { get; }
        
    }
}