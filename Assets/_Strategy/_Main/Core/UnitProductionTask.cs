using _Strategy._Main.Abstractions;
using UnityEngine;


namespace _Strategy._Main.Core
{
    
    public sealed class UnitProductionTask : IUnitProductionTask
    {

        public GameObject UnitPrefab { get; }
        public Sprite Icon { get; }
        public string UnitName { get; }
        public float TimeLeft { get; set; }
        public float ProductionTime { get; }

        
        public UnitProductionTask(GameObject unitPrefab, Sprite icon, string name, float productionTime)
        {
            UnitPrefab = unitPrefab;
            Icon = icon;
            UnitName = name;
            TimeLeft = productionTime;
            ProductionTime = productionTime;
        }
        
        
    }
}