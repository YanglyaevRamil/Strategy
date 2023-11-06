using UnityEngine;


namespace _Strategy._Main.Abstractions.Commands
{
    
    public interface IProduceUnitCommand : ICommand, IIconHandler
    {

        GameObject UnitPrefab { get; }
        
        string UnitName { get; }
        
        float ProductionTime { get; }
        
    }
}