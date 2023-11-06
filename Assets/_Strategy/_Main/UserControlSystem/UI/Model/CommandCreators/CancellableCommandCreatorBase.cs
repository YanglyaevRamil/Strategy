using System;
using System.Threading;
using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.Utils.AsyncExtensions;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Model.CommandCreators
{
    
    public abstract class CancellableCommandCreatorBase<TCommand, TArgument> : CommandCreatorBase<TCommand> where TCommand : ICommand
    {

        [Inject] private IAwaitable<TArgument> _awaitableArgument;

        private CancellationTokenSource _ctSource;

        
        protected sealed override async void ClassSpecificCommandCreation(Action<TCommand> creationCallback)
        {
            _ctSource = new CancellationTokenSource();
            try
            {
                var awaitableArgument = await _awaitableArgument.WithCancellation(_ctSource.Token);
                creationCallback?.Invoke(CreateCommand(awaitableArgument));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        protected abstract TCommand CreateCommand(TArgument argument);


        public override void ProcessCancel()
        {
            base.ProcessCancel();

            if (_ctSource != null)
            {
                _ctSource.Cancel();
                _ctSource.Dispose();
                _ctSource = null;
            }
        }


    }
}