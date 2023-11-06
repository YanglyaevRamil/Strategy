using System;
using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.UserControlSystem.Commands;
using _Strategy._Main.Utils.AssetsInjector;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Model.CommandCreators
{
    
    public sealed class PatrolUnitCommandCreator : CancellableCommandCreatorBase<IPatrolCommand, Vector3>
    {

        // [Inject] private AssetsContext _context;
        [Inject] private SelectableValue _selectable;
        
        // private event Action<IPatrolCommand> _creationCallback;


        // [Inject]
        // private void Init(Vector3Value groundClicks) => groundClicks.OnNewValueChanged += OnNewValue;
        //
        //
        // private void OnNewValue(Vector3 groundClick)
        // {
        //     _creationCallback?.Invoke(new PatrolUnitCommand(_selectable.CurrentValue.PivotPoint.position, groundClick));
        //     _creationCallback = null;
        // }


        protected override IPatrolCommand CreateCommand(Vector3 argument) =>
            new PatrolUnitCommand(_selectable.CurrentValue.PivotPoint.position, argument);
    }
}