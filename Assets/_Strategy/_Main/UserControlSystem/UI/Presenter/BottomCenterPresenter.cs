using System;
using _Strategy._Main.UserControlSystem.UI.Model;
using _Strategy._Main.UserControlSystem.UI.View;
using UniRx;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Presenter
{
    
    public class BottomCenterPresenter : MonoBehaviour
    {

        [SerializeField] private GameObject _uiHolder;

        private IDisposable _productionQueueAddCt;
        private IDisposable _productionQueueRemoveCt;
        private IDisposable _productionQueueReplaceCt;
        private IDisposable _cancelButtonCts;


        
        [Inject]
        private void Init(BottomCenterModel model, BottomCenterView view)
        {

            model.UnitProducer.Subscribe(unitProducer =>
            {
                _productionQueueAddCt?.Dispose();
                _productionQueueRemoveCt?.Dispose();
                _productionQueueReplaceCt?.Dispose();
                _cancelButtonCts?.Dispose();

                view.Clear();
                _uiHolder.SetActive(unitProducer != null);

                if (unitProducer != null)
                {
                    _productionQueueAddCt = unitProducer.UnitProductionQueue
                        .ObserveAdd()
                        .Subscribe(addEvent => view.SetTask(addEvent.Value, addEvent.Index));

                    _productionQueueRemoveCt = unitProducer.UnitProductionQueue
                        .ObserveRemove()
                        .Subscribe(removeEvent => view.SetTask(null, removeEvent.Index));

                    _productionQueueReplaceCt = unitProducer.UnitProductionQueue
                        .ObserveReplace()
                        .Subscribe(replaceEvent => view.SetTask(replaceEvent.NewValue, replaceEvent.Index));

                    _cancelButtonCts = view.CancelButtonClicks.Subscribe(unitProducer.Cancel);

                    for (int i = 0; i < unitProducer.UnitProductionQueue.Count; i++)
                    {
                        view.SetTask(unitProducer.UnitProductionQueue[i], i);
                    }
                }
            });
        }

        
    }
}