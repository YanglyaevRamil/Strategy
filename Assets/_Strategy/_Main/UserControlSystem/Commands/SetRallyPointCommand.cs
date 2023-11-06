using _Strategy._Main.Abstractions.Commands;
using UnityEngine;


namespace _Strategy._Main.UserControlSystem.Commands
{
    
    public sealed class SetRallyPointCommand : ISetRallyPointCommand
    {
        
        public Vector3 RallyPoint { get; }

        
        public SetRallyPointCommand(Vector3 rallyPoint)
        {
            RallyPoint = rallyPoint;
        }
        
        
    }
}