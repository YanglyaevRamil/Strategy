using System;
using System.Threading;
using System.Threading.Tasks;
using _Strategy._Main.Abstractions;
using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.Utils.AsyncExtensions;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;


namespace _Strategy._Main.Core.CommandExecutors
{
    
    public sealed class AttackCommandExecutor : CommandExecutorBase<IAttackCommand>
    {

        [SerializeField] private Animator _animator;
        [SerializeField] private StopCommandExecutor _stopCommandExecutor;

        [Inject] private IHealthHolder _ourHealth;
        [Inject(Id = "AttackDistance")] private float _attackingDistance;
        [Inject(Id = "AttackPeriod")] private int _attackingPeriod;

        private readonly Subject<Vector3> _targetPositions = new();
        private readonly Subject<Quaternion> _targetRotations = new();
        private readonly Subject<IAttackable> _attackTargets = new();

        private NavMeshAgent _navAgent;
        
        private Vector3 _ourPosition;
        private Vector3 _targetPosition;
        private Quaternion _ourRotation;

        private Transform _targetTransform;
        
        private AttackOperation _currentAttackOperation;

        private object _locker = new();
        
        
        [Inject]
        private void Init()
        {
            _targetPositions.Select(value =>
                    new Vector3(
                        (float) Math.Round(value.x, 2),
                        (float) Math.Round(value.y, 2),
                        (float) Math.Round(value.z, 2)))
                .Distinct()
                .ObserveOnMainThread()
                .Subscribe(StartMovingToPosition);

            _attackTargets
                .ObserveOnMainThread()
                .Subscribe(StartAttackingTargets);

            _targetRotations
                .ObserveOnMainThread()
                .Subscribe(SetAttackRotation);
        }
        
        
        
        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
        }


        private void Update()
        {
            if (_currentAttackOperation != null)
            {
                lock (_locker)
                {
                    _ourPosition = transform.position;
                    _ourRotation = transform.rotation;
                    if (_targetTransform != null)
                    {
                        _targetPosition = _targetTransform.position;
                    }
                }
            }
        }
        

        private void SetAttackRotation(Quaternion targetRotation)
        {
            transform.rotation = targetRotation;
        }


        private void StartAttackingTargets(IAttackable target)
        {
            _navAgent.isStopped = true;
            _navAgent.ResetPath();
            _animator.SetTrigger(Animator.StringToHash(AnimationTypes.Attack.ToString()));
            target.ReceiveDamage(GetComponent<IDamageDealer>().Damage);
        }


        private void StartMovingToPosition(Vector3 position)
        {
            _navAgent.destination = position;
            _animator.SetTrigger(Animator.StringToHash(AnimationTypes.Walk.ToString()));
        }
        

        protected override async Task ExecuteSpecificCommand(IAttackCommand command)
        {
            Debug.Log($"[{name}.{GetInstanceID()}] Attacks {command.Target} with HP [{command.Target.Health}/{command.Target.MaxHealth}]");

            _targetTransform = (command.Target as Component).transform;
            _currentAttackOperation = new AttackOperation(this, command.Target);
            Update();
            _stopCommandExecutor.CancellationTokenSource = new CancellationTokenSource();

            try
            {
                await _currentAttackOperation.WithCancellation(_stopCommandExecutor.CancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                _currentAttackOperation.Cancel();
            }
            
            _animator.SetTrigger(Animator.StringToHash(AnimationTypes.Idle.ToString()));
            _currentAttackOperation = null;
            _targetTransform = null;
            _stopCommandExecutor.CancellationTokenSource = null;
        }
        
        
        
        private sealed class AttackOperation : IAwaitable<AsyncExtensions.Void>
        {
            
            private readonly AttackCommandExecutor _attackCommandExecutor;
            private readonly IAttackable _target;

            private bool _isCancelled;

            private event Action OnComplete;


            public AttackOperation(AttackCommandExecutor attackCommandExecutor, IAttackable target)
            {
                _target = target;
                _attackCommandExecutor = attackCommandExecutor;

                var attackThread = new Thread(AttackAlgorithm);
                attackThread.Start();
            }


            private void AttackAlgorithm(object obj)
            {
                while (true)
                {
                    if (_attackCommandExecutor == null ||
                        _attackCommandExecutor._ourHealth.Health == 0 ||
                        _target.Health == 0 ||
                        _isCancelled)
                    {
                        OnComplete?.Invoke();
                        return;
                    }

                    var targetPosition = default(Vector3);
                    var ourPosition = default(Vector3);
                    var ourRotation = default(Quaternion);

                    lock (_attackCommandExecutor._locker)
                    {
                        targetPosition = _attackCommandExecutor._targetPosition;
                        ourPosition = _attackCommandExecutor._ourPosition;
                        ourRotation = _attackCommandExecutor._ourRotation;
                    }

                    var direction = targetPosition - ourPosition;
                    var distanceToTarget = direction.magnitude;

                    if (distanceToTarget > _attackCommandExecutor._attackingDistance)
                    {
                        var finalDestination = targetPosition -
                                            direction.normalized * (_attackCommandExecutor._attackingDistance * 0.9f);
                        
                        _attackCommandExecutor._targetPositions.OnNext(finalDestination);
                        Thread.Sleep(100);
                        
                    } else if (ourRotation != Quaternion.LookRotation(direction))
                    {
                        _attackCommandExecutor._targetRotations.OnNext(Quaternion.LookRotation(direction));
                    }
                    else
                    {
                        _attackCommandExecutor._attackTargets.OnNext(_target);
                        Thread.Sleep(_attackCommandExecutor._attackingPeriod);
                    }
                    
                }
            }


            public void Cancel()
            {
                _isCancelled = true;
                OnComplete?.Invoke();
            }
            
            
            public IAwaiter<AsyncExtensions.Void> GetAwaiter()
            {
                return new AttackOperationAwaiter(this);
            }
            
            
            private class AttackOperationAwaiter : AwaiterBase<AsyncExtensions.Void>
            {

                private AttackOperation _attackOperation;
                
                
                public AttackOperationAwaiter(AttackOperation attackOperation)
                {
                    _attackOperation = attackOperation;
                    attackOperation.OnComplete += OnComplete;
                }


                private void OnComplete()
                {
                    _attackOperation.OnComplete -= OnComplete;
                    OnWaitFinish(new AsyncExtensions.Void());
                }
                
            }
            
        }
        
        
    }
}