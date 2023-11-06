namespace _Strategy._Main.Abstractions
{
    
    public interface IUnitProductionTask : IIconHandler
    {

        public string UnitName { get; }

        public float TimeLeft { get; }

        public float ProductionTime { get; }

    }
}