using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.Core.CommandExecutors;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.Core
{
    
    public sealed class MainBuildingCommandsQueue : MonoBehaviour, ICommandsQueue
    {

        [Inject]
        private CommandExecutorBase<IProduceUnitCommand> _produceUnitCommandExecutor;

        [Inject]
        private CommandExecutorBase<ISetRallyPointCommand> _setRallyPointExecutor;
        
        
        public ICommand CurrentCommand => default;
        

        
        public async void EnqueueCommand(object command)
        {
            await _produceUnitCommandExecutor.TryExecuteCommand(command);
            await _setRallyPointExecutor.TryExecuteCommand(command);
        }
        
        
        public void Clear() { }

        
    }
}