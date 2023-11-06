using System;
using _Strategy._Main.Abstractions;
using UniRx;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Model
{
    
    public sealed class BottomCenterModel
    {

        public IObservable<IUnitProducer> UnitProducer { get; private set; }


        [Inject]
        public void Init(IObservable<ISelectable> currentlySelected)
        {
            UnitProducer = currentlySelected
                .Select(selectable => selectable as Component)
                .Select(component => component != null ? component.GetComponent<IUnitProducer>() : default);
        }
        

    }
}