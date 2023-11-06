using UnityEngine;


namespace _Strategy._Main.Abstractions.Commands
{
    
    public interface ISetRallyPointCommand : ICommand
    {
        
        Vector3 RallyPoint { get; }
        
    }
}