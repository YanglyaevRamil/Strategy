using UniRx;


namespace _Strategy._Main.Abstractions
{
    
    public interface IUnitProducer
    {

        public IReadOnlyReactiveCollection<IUnitProductionTask> UnitProductionQueue { get; }
        
        public void Cancel(int index);

    }
}