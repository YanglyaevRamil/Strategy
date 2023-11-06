using System;
using _Strategy._Main.Abstractions;
using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.UserControlSystem.UI.Model;
using _Strategy._Main.UserControlSystem.UI.View;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Presenter
{
    
    internal sealed class CommandClicksPresenter : MonoBehaviour
    {
        
        [SerializeField] private CommandButtonsView _commandButtonsView;

        [Inject] private IObservable<ISelectable> _selectableStream;
        [Inject(Id="Commands")] private IObservable<Vector3> _commandClicksRMB;
        [Inject(Id="GroundClicks")] private Vector3Value _groundClicksRMB;
        
        [Inject] private CommandButtonsModel _commandButtonsModel;
        
        private ISelectable _currentSelectable;

        
        
        private void Start()
        {
            _commandClicksRMB.Subscribe(OnNewGroundedCommandClick);
            _selectableStream.Subscribe(OnNewSelectable);
        }


        private void OnDestroy()
        {
            
        }


        private void OnNewGroundedCommandClick(Vector3 newPosition)
        {
            //Need To call OnCommandButtonClicked in Model to trigger other view events 
            Debug.Log($"Observe works! [{newPosition}]");
            if (_currentSelectable != null)
            {

                if (CommandButtonsModel.SelectableMoveButtonClickedDict.TryGetValue(_currentSelectable, out bool moveButtonPressed))
                {
                    if (!moveButtonPressed)
                    {
                        var moveCommandExecutor = (_currentSelectable as Component).GetComponentInParent<ICommandExecutor<IMoveCommand>>();
                        var queue = (_currentSelectable as Component).GetComponentInParent<ICommandsQueue>();
                        if (moveCommandExecutor != null)
                        {
                            _commandButtonsModel.OnCommandButtonClicked(moveCommandExecutor, queue);
                            _groundClicksRMB.SetValue(newPosition);
                        }
                    }
                    else
                    {
                        CommandButtonsModel.SelectableMoveButtonClickedDict[_currentSelectable] = false;
                    }
                }
                else
                {
                    var moveCommandExecutor = (_currentSelectable as Component).GetComponentInParent<ICommandExecutor<IMoveCommand>>();
                    var queue = (_currentSelectable as Component).GetComponentInParent<ICommandsQueue>();
                    if (moveCommandExecutor != null)
                    {
                        _commandButtonsModel.OnCommandButtonClicked(moveCommandExecutor, queue);
                        _groundClicksRMB.SetValue(newPosition);
                    }
                }
            }
        }


        private void OnNewSelectable(ISelectable selectable)
        {
            if (_currentSelectable != selectable)
            {
                _currentSelectable = selectable;
            }
        }
        
        // TODO fix button and command reflection, button disabling
        
        
    }
}