using System.Threading;
using System.Threading.Tasks;
using _Strategy._Main.Abstractions.Commands;
using UnityEngine;


namespace _Strategy._Main.Core.CommandExecutors
{
    
    public sealed class StopCommandExecutor : CommandExecutorBase<IStopCommand>
    {

        public CancellationTokenSource CancellationTokenSource { get; set; }

        
        protected override async Task ExecuteSpecificCommand(IStopCommand command)
        {
            CancellationTokenSource?.Cancel();
        }
        
        
    }
}