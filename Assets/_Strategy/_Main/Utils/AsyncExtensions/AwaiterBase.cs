using System;


namespace _Strategy._Main.Utils.AsyncExtensions
{
    
    public abstract class AwaiterBase<TAwaited> : IAwaiter<TAwaited>
    {

        private TAwaited _result;

        private bool _isCompleted;

        private event Action _continuation;

        
        public bool IsCompleted => _isCompleted;

        public TAwaited GetResult() => _result;
        
        
        
        public void OnCompleted(Action continuation)
        {
            if (_isCompleted)
                continuation?.Invoke();
            else
                _continuation = continuation;
        }


        protected void OnWaitFinish(TAwaited result)
        {
            _result = result;
            _isCompleted = true;
            _continuation?.Invoke();
        }
        
        
    }
}