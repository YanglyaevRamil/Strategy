using UnityEngine;


namespace _Strategy._Main.Abstractions.Commands
{
    
    public interface IPatrolCommand : ICommand
    {

        Vector3 From { get; }

        Vector3 To { get; }

        
    }
}