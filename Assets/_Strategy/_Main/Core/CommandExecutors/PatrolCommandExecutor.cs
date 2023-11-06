using System;
using System.Threading;
using System.Threading.Tasks;
using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.Utils.AsyncExtensions;
using UnityEngine;
using UnityEngine.AI;


namespace _Strategy._Main.Core.CommandExecutors
{
    
    public sealed class PatrolCommandExecutor : CommandExecutorBase<IPatrolCommand>
    {

        [SerializeField] private Animator _animator;
        
        [SerializeField] private UnitMovementStop _unitMovementStop;
        [SerializeField] private StopCommandExecutor _stopCommandExecutor;

        private NavMeshAgent _navAgent;
        
        
        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
        }


        protected override async Task ExecuteSpecificCommand(IPatrolCommand command)
        {
            Debug.Log($"{name} Patrols from [{command.From}] to [{command.To}]");

            var pointFrom = command.From;
            var pointTo = command.To;

            while (true)
            {
                _navAgent.destination = pointTo;
                _animator.SetTrigger(Animator.StringToHash(AnimationTypes.Walk.ToString()));
                _stopCommandExecutor.CancellationTokenSource = new CancellationTokenSource();

                try
                {
                    await _unitMovementStop.WithCancellation(_stopCommandExecutor.CancellationTokenSource.Token);
                }
                catch
                {
                    _navAgent.isStopped = true;
                    _navAgent.ResetPath();
                    break;
                }

                (pointFrom, pointTo) = (pointTo, pointFrom);
            }

            _stopCommandExecutor.CancellationTokenSource = null;
            _animator.SetTrigger(Animator.StringToHash(AnimationTypes.Idle.ToString()));
        }
        
        
    }
}