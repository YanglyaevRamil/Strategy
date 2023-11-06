using System.Threading.Tasks;
using _Strategy._Main.Abstractions;
using _Strategy._Main.Abstractions.Commands;
using UniRx;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.Core.CommandExecutors
{
    
    public sealed class ProduceUnitCommandExecutor : CommandExecutorBase<IProduceUnitCommand>, IUnitProducer
    {

        [SerializeField] private Transform _unitsParent;
        [SerializeField] private int _maxUnitsInQueue = 6;
        
        [Inject] private DiContainer _diContainer;


        private ReactiveCollection<IUnitProductionTask> _unitProductionQueue = new();
        
        public IReadOnlyReactiveCollection<IUnitProductionTask> UnitProductionQueue => _unitProductionQueue;

        
        private void Start()
        {
            Observable.EveryUpdate().Subscribe(_ => { Tick(); }).AddTo(this);
        }

        
        private void Tick()
        {
            if (_unitProductionQueue.Count != 0)
            {
                var innerTask = (UnitProductionTask) _unitProductionQueue[0];
                innerTask.TimeLeft -= Time.deltaTime;

                if (innerTask.TimeLeft <= 0)
                {
                    RemoveTaskAtIndex(0);
                    
                    var instance = _diContainer.InstantiatePrefab(
                            innerTask.UnitPrefab, 
                            transform.position, 
                            Quaternion.identity,
                            _unitsParent
                        );
                        
                    instance.name = innerTask.UnitPrefab.name;
                    var fractionMember = instance.GetComponent<FractionMember>();
                    fractionMember.SetFraction(GetComponent<FractionMember>().FractionId);
                    
                    var queue = instance.GetComponent<ICommandsQueue>();
                    var mainBuilding = GetComponent<MainBuilding>();
                    queue.EnqueueCommand(new MoveUnitCommand(mainBuilding.RallyPoint));
                }
            }
        }


        [ContextMenu("ProduceUnit")]
        protected override async Task ExecuteSpecificCommand(IProduceUnitCommand command)
        {
            _unitProductionQueue.Add(
                new UnitProductionTask(
                    command.UnitPrefab, 
                    command.Icon, 
                    command.UnitName,
                    command.ProductionTime));
        }
            


        private void RemoveTaskAtIndex(int index)
        {
            for (int i = index; i < _unitProductionQueue.Count - 1; i++)
            {
                _unitProductionQueue[i] = _unitProductionQueue[i + 1];
            }
            
            _unitProductionQueue.RemoveAt(_unitProductionQueue.Count - 1);
        }
        
        
        public void Cancel(int index) => RemoveTaskAtIndex(index);


    }
}