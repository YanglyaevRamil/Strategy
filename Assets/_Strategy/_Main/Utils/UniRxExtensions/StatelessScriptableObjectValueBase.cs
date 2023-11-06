using System;
using _Strategy._Main.UserControlSystem.UI.Model;
using UniRx;


namespace _Strategy._Main.Utils.UniRxExtensions
{
    
    public abstract class StatelessScriptableObjectValueBase<T> : ScriptableObjectValueBase<T>, IObservable<T>
    {

        private Subject<T> _innerDataSource = new Subject<T>();

        
        public override void SetValue(T value)
        {
            base.SetValue(value);
            _innerDataSource.OnNext(value);
        }


        public IDisposable Subscribe(IObserver<T> observer) => _innerDataSource.Subscribe(observer);

        
    }
}