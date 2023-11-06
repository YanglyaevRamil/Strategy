using UnityEngine;


namespace _Strategy._Main.Abstractions.Commands
{
    
    public interface IMoveCommand : ICommand
    {
        public Vector3 Target { get; }
        
    }
}