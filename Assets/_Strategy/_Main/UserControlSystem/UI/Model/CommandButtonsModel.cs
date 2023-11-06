using System;
using System.Collections.Generic;
using _Strategy._Main.Abstractions;
using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.UserControlSystem.UI.Model.CommandCreators;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Model
{
    
    public sealed class CommandButtonsModel
    {
        
        [Inject] private CommandCreatorBase<IProduceUnitCommand> _unitProducer;
        [Inject] private CommandCreatorBase<IAttackCommand> _attacker;
        [Inject] private CommandCreatorBase<IStopCommand> _stopper;
        [Inject] private CommandCreatorBase<IMoveCommand> _mover;
        [Inject] private CommandCreatorBase<IPatrolCommand> _patroller;
        [Inject] private CommandCreatorBase<ISetRallyPointCommand> _rallyPointer;

        private bool _commandIsPending;

        public event Action<ICommandExecutor> OnCommandAccepted = delegate(ICommandExecutor executor) {  };
        public event Action OnCommandSent = () => { };
        public event Action OnCommandCancel = () => { };


        public static Dictionary<ISelectable, bool> SelectableMoveButtonClickedDict = new();
        
        
        public event Action<bool> OnMoveButtonPressed = delegate {  };



        public void OnCommandButtonClicked(ICommandExecutor commandExecutor, ICommandsQueue commandsQueue)
        {
            if (_commandIsPending)
                ProcessOnCancel();

            _commandIsPending = true;
            OnCommandAccepted(commandExecutor);

            _unitProducer.ProcessCommandExecutor(commandExecutor,
                command => ExecuteCommandWrapper(command, commandsQueue));
            
            _attacker.ProcessCommandExecutor(commandExecutor,
                command => ExecuteCommandWrapper(command, commandsQueue));
            
            _stopper.ProcessCommandExecutor(commandExecutor,
                command => ExecuteCommandWrapper(command, commandsQueue));
            
            _mover.ProcessCommandExecutor(commandExecutor,
                command => ExecuteCommandWrapper(command, commandsQueue));

            _patroller.ProcessCommandExecutor(commandExecutor,
                command => ExecuteCommandWrapper(command, commandsQueue));

            _rallyPointer.ProcessCommandExecutor(commandExecutor,
                command => ExecuteCommandWrapper(command, commandsQueue));
        }

        
        public void ExecuteCommandWrapper(object command, ICommandsQueue commandsQueue)
        {

            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                commandsQueue.Clear();
            }
            
            commandsQueue.EnqueueCommand(command);
            _commandIsPending = false;
            OnCommandSent();
        }


        public void OnSelectionChanged()
        {
            _commandIsPending = false;
            ProcessOnCancel();
        }


        private void ProcessOnCancel()
        {
            _unitProducer.ProcessCancel(); 
            _attacker.ProcessCancel();
            _stopper.ProcessCancel(); 
            _mover.ProcessCancel(); 
            _patroller.ProcessCancel(); 
            _rallyPointer.ProcessCancel();
            
            OnCommandCancel();
        }

        
    }
}