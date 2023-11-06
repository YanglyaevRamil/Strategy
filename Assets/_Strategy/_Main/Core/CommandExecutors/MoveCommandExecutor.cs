using System;
using System.Threading;
using System.Threading.Tasks;
using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.Utils.AsyncExtensions;
using UnityEngine;
using UnityEngine.AI;


namespace _Strategy._Main.Core.CommandExecutors
{
    
    public sealed class MoveCommandExecutor : CommandExecutorBase<IMoveCommand>
    {
        
        [SerializeField] private Animator _animator;
        
        [SerializeField] private UnitMovementStop _movementStop;
        [SerializeField] private StopCommandExecutor _stopCommandExecutor;

        private NavMeshAgent _navAgent;
        
        private int _walkTrigger;
        private int _idleTrigger;


        private void Awake()
        {
            _walkTrigger = Animator.StringToHash(AnimationTypes.Walk.ToString());
            _idleTrigger = Animator.StringToHash(AnimationTypes.Idle.ToString());
            _navAgent = GetComponent<NavMeshAgent>();
        }


        protected override async Task ExecuteSpecificCommand(IMoveCommand command)
        {
            _navAgent.destination = command.Target;
            _animator.SetTrigger(_walkTrigger);

            _stopCommandExecutor.CancellationTokenSource = new CancellationTokenSource();
            try
            {
                await _movementStop.WithCancellation(_stopCommandExecutor.CancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                _navAgent.isStopped = true;
                _navAgent.ResetPath();
            }

            _stopCommandExecutor.CancellationTokenSource = null;
            _animator.SetTrigger(_idleTrigger);
        }
        
        
        
    }
}