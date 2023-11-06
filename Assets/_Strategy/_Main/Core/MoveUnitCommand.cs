using _Strategy._Main.Abstractions.Commands;
using UnityEngine;


namespace _Strategy._Main.Core
{
    
    public sealed class MoveUnitCommand : IMoveCommand
    {
        
        public Vector3 Target { get; }

        
        public MoveUnitCommand(Vector3 target)
        {
            Target = target;
        }
        
        
    }
}