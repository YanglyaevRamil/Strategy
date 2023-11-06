using System;
using System.Collections.Generic;
using _Strategy._Main.Abstractions;
using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.UserControlSystem.UI.Model;
using _Strategy._Main.UserControlSystem.UI.View;
using UniRx;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Presenter
{
    
    internal sealed class CommandButtonsPresenter : MonoBehaviour
    {

        [SerializeField] private CommandButtonsView _commandButtonsView;

        [Inject] private IObservable<ISelectable> _selectable;
        [Inject] private CommandButtonsModel _commandButtonsModel;

        private ISelectable _currentSelectable;
        
        
        
        private void Start()
        {
            _commandButtonsView.OnClickSubscription += _commandButtonsModel.OnCommandButtonClicked;
            _commandButtonsView.OnClickSubscription += OnCommandButtonClickResolver;
            _commandButtonsModel.OnCommandSent += _commandButtonsView.UnblockAllInteractions;
            _commandButtonsModel.OnCommandCancel += _commandButtonsView.UnblockAllInteractions;
            _commandButtonsModel.OnCommandAccepted += _commandButtonsView.BlockInteractions;

            _selectable.Subscribe(OnNewValueSubscribe);
        }


        private void OnDestroy()
        {
            _commandButtonsView.OnClickSubscription -= _commandButtonsModel.OnCommandButtonClicked;
            _commandButtonsView.OnClickSubscription -= OnCommandButtonClickResolver;
            _commandButtonsModel.OnCommandSent -= _commandButtonsView.UnblockAllInteractions;
            _commandButtonsModel.OnCommandCancel -= _commandButtonsView.UnblockAllInteractions;
            _commandButtonsModel.OnCommandAccepted -= _commandButtonsView.BlockInteractions;
        }


        private void OnNewValueSubscribe(ISelectable selectable)
        {
            if (_currentSelectable != selectable)
            {
                
                _commandButtonsModel.OnSelectionChanged();
                
                _currentSelectable = selectable;
                _commandButtonsView.ClearButtonsPanel();

                if (selectable != null)
                {
                    var commandExecutors = new List<ICommandExecutor>();
                    commandExecutors.AddRange
                    (
                        (selectable as Component).GetComponentsInParent<ICommandExecutor>()
                    );

                    var queue = (selectable as Component).GetComponentInParent<ICommandsQueue>();
                    _commandButtonsView.MakeLayout(commandExecutors, queue);
                }
            }
        }

        
        private void OnCommandButtonClickResolver(ICommandExecutor commandExecutor, ICommandsQueue commandsQueue)
        {
            if (commandExecutor is ICommandExecutor<IMoveCommand> moveExecutor)
            {
                var selectable = (commandExecutor as Component).GetComponentInParent<ISelectable>();
                CommandButtonsModel.SelectableMoveButtonClickedDict[selectable] = true;
            }
        }



    }
}