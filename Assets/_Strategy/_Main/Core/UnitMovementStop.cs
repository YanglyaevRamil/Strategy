using System;
using _Strategy._Main.Abstractions;
using _Strategy._Main.Utils.AsyncExtensions;
using _Strategy._Main.Utils.GOExtensions;
using UniRx;
using UnityEngine;
using UnityEngine.AI;


namespace _Strategy._Main.Core
{
    
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class UnitMovementStop : MonoBehaviour, IAwaitable<AsyncExtensions.Void>
    {
        
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private CollisionDetector _collisionDetector;

        [SerializeField] private int _throttleFrames = 60;
        [SerializeField] private int _continiusThreshold = 10;
        
        public event Action OnStop;
        
        public IAwaiter<AsyncExtensions.Void> GetAwaiter() => new StopAwaiter(this);

        
        
        private void OnValidate() => _agent ??= GetComponent<NavMeshAgent>();


        private void Awake()
        {
            _collisionDetector.Collisions
                .Where(_ => _agent.hasPath)
                .Where(collision => collision.collider.GetComponentInParent<IUnit>() != null)
                .Select(_ => Time.frameCount)
                .Distinct()
                .Buffer(_throttleFrames)
                .Where(buffer =>
                {
                    for (int i = 1; i < buffer.Count; i++)
                    {
                        if (buffer[i] - buffer[i - 1] > _continiusThreshold)
                            return false;
                    }
                    return true;
                })
                .Subscribe(_ =>
                {
                    _agent.isStopped = true;
                    _agent.ResetPath();
                    OnStop?.Invoke();
                })
                .AddTo(this);
        }


        private void Update()
        {
            if (!_agent.pathPending)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0.0f)
                    {
                        OnStop?.Invoke();
                    }
                }
            }
        }
        
        
        
        private sealed class StopAwaiter : AwaiterBase<AsyncExtensions.Void>
        {
            
            private readonly UnitMovementStop _unitMovementStop;


            public StopAwaiter(UnitMovementStop unitMovementStop)
            {
                _unitMovementStop = unitMovementStop;
                _unitMovementStop.OnStop += OnStop;
            }
            
            
            private void OnStop()
            {
                _unitMovementStop.OnStop -= OnStop;
                OnWaitFinish(new AsyncExtensions.Void());
            }
        }
        
        
    }
}