using System.Runtime.CompilerServices;


namespace _Strategy._Main.Utils.AsyncExtensions
{
    
    public interface IAwaiter<TAwaited> : INotifyCompletion
    {
        
        bool IsCompleted { get; }
        
        TAwaited GetResult();
        
    }
}