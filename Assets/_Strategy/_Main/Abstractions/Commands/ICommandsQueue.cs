namespace _Strategy._Main.Abstractions.Commands
{
    
    public interface ICommandsQueue
    {
        
        void EnqueueCommand(object command);

        ICommand CurrentCommand { get; }

        void Clear();

    }
}