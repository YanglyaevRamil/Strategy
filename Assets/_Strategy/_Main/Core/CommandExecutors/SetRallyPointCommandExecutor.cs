using System.Threading.Tasks;
using _Strategy._Main.Abstractions.Commands;
using UnityEngine;


namespace _Strategy._Main.Core.CommandExecutors
{
    
    public sealed class SetRallyPointCommandExecutor : CommandExecutorBase<ISetRallyPointCommand>
    {
        
        protected override async Task ExecuteSpecificCommand(ISetRallyPointCommand command)
        {
            GetComponent<MainBuilding>().RallyPoint = command.RallyPoint;
            Debug.Log($"Rally Point: [{command.RallyPoint}]");
        }
        
        
    }
}