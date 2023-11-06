using System;
using _Strategy._Main.UserControlSystem.UI.Model;
using UniRx;


namespace _Strategy._Main.Utils.UniRxExtensions
{
    
    public abstract class StatefullScriptableObjectValueBase<T> : ScriptableObjectValueBase<T>, IObservable<T>
    {

        private ReactiveProperty<T> _innerDataSource = new ReactiveProperty<T>();

        
        public override void SetValue(T value)
        {
            base.SetValue(value);
            _innerDataSource.Value = value;
        }


        public IDisposable Subscribe(IObserver<T> observer) => _innerDataSource.Subscribe(observer);

        
    }
}