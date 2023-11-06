using _Strategy._Main.Abstractions.Commands;
using UnityEngine;


namespace _Strategy._Main.UserControlSystem.Commands
{
    
    public sealed class PatrolUnitCommand : IPatrolCommand
    {
        
        public Vector3 From { get; }
        
        public Vector3 To { get; }


        public PatrolUnitCommand(Vector3 from, Vector3 to)
        {
            From = from;
            To = to;
        }
        
        
    }
}