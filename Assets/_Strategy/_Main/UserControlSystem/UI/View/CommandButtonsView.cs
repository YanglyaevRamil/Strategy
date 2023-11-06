using System;
using System.Collections.Generic;
using System.Linq;
using _Strategy._Main.Abstractions.Commands;
using UnityEngine;
using UnityEngine.UI;


namespace _Strategy._Main.UserControlSystem.UI.View
{
    
    public sealed class CommandButtonsView : MonoBehaviour
    {

        [SerializeField] private GameObject _attackButton;
        [SerializeField] private GameObject _moveButton;
        [SerializeField] private GameObject _patrolButton;
        [SerializeField] private GameObject _stopButton;
        [SerializeField] private GameObject _produceUnitButton;
        [SerializeField] private GameObject _setRallyPointButton;

        
        public event Action<ICommandExecutor, ICommandsQueue> OnClickSubscription = delegate(ICommandExecutor executor, ICommandsQueue queue) { };

        
        private Dictionary<Type, GameObject> _buttonsByExecutorType;
        

        private void Awake()
        {
            _buttonsByExecutorType = new Dictionary<Type, GameObject>
            {
                [typeof(ICommandExecutor<IAttackCommand>)] = _attackButton,
                [typeof(ICommandExecutor<IMoveCommand>)] = _moveButton,
                [typeof(ICommandExecutor<IPatrolCommand>)] = _patrolButton,
                [typeof(ICommandExecutor<IStopCommand>)] = _stopButton,
                [typeof(ICommandExecutor<IProduceUnitCommand>)] = _produceUnitButton,
                [typeof(ICommandExecutor<ISetRallyPointCommand>)] = _setRallyPointButton
            };
        }


        private void OnDestroy()
        {
            ClearButtonsPanel();
            _buttonsByExecutorType.Clear();
        }


        public void BlockInteractions(ICommandExecutor commandExecutor)
        {
            UnblockAllInteractions();
            GetButtonByType(commandExecutor.GetType())
                .GetComponent<Selectable>()
                .interactable = false;
        }


        public void UnblockAllInteractions() => SetInteractable(true);


        private void SetInteractable(bool isInteractable)
        {
            _attackButton.GetComponent<Selectable>().interactable = isInteractable;
            _moveButton.GetComponent<Selectable>().interactable = isInteractable;
            _patrolButton.GetComponent<Selectable>().interactable = isInteractable;
            _stopButton.GetComponent<Selectable>().interactable = isInteractable;
            _produceUnitButton.GetComponent<Selectable>().interactable = isInteractable;
            _setRallyPointButton.GetComponent<Selectable>().interactable = isInteractable;
        }


        public void MakeLayout(List<ICommandExecutor> commandExecutors, ICommandsQueue queue)
        {
            for (int i = 0; i < commandExecutors.Count; i++)
            {
                var effectiveCounter = i;
                var currentExecutor = commandExecutors[effectiveCounter];

                var buttonGameObject = GetButtonByType(currentExecutor.GetType());
                
                buttonGameObject.SetActive(true);
                
                var button = buttonGameObject.GetComponent<Button>();
                button.onClick.AddListener(() => OnClickSubscription(currentExecutor, queue));
            }
        }


        public GameObject GetButtonByType(Type executorInstanceType)
        {
            return _buttonsByExecutorType
                .First(type => type
                    .Key
                    .IsAssignableFrom(executorInstanceType))
                .Value;
        }


        public void ClearButtonsPanel()
        {
            foreach (var kvp in _buttonsByExecutorType)
            {
                kvp.Value.GetComponent<Button>().onClick.RemoveAllListeners();
                kvp.Value.SetActive(false);
            }
        }

        
    }
}