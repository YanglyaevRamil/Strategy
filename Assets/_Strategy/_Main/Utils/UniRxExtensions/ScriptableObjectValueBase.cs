using System;
using _Strategy._Main.Utils.AsyncExtensions;
using UnityEngine;


namespace _Strategy._Main.UserControlSystem.UI.Model
{
    
    public abstract class ScriptableObjectValueBase<T> : ScriptableObject, IAwaitable<T>
    {
        
        public T CurrentValue { get; private set; }
        
        public event Action<T> OnNewValueChanged = delegate(T obj) {  };

        
        public virtual void SetValue(T value)
        {
            CurrentValue = value;
            OnNewValueChanged(value);
        }


        public IAwaiter<T> GetAwaiter() => new NewValueNotifier<T>(this);


        private sealed class NewValueNotifier<TAwaited> : AwaiterBase<TAwaited>
        {
            
            private readonly ScriptableObjectValueBase<TAwaited> _scriptableObjectValueBase;

            
            public NewValueNotifier(ScriptableObjectValueBase<TAwaited> scriptableObjectValueBase)
            {
                _scriptableObjectValueBase = scriptableObjectValueBase;
                _scriptableObjectValueBase.OnNewValueChanged += OnNewValue;
            }
            
            
            private void OnNewValue(TAwaited obj)
            {
                _scriptableObjectValueBase.OnNewValueChanged -= OnNewValue;
                OnWaitFinish(obj);
            }
        }
        
        
    }
}