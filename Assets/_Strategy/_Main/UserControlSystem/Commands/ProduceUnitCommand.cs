using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.Utils.AssetsInjector;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.UserControlSystem.Commands
{
    
    public class ProduceUnitCommand : IProduceUnitCommand
    {
        
        [InjectAsset("ChomperUnit")] private GameObject _unitPrefab;
        
        [Inject(Id = "Chomper")] public Sprite Icon { get; }
        [Inject(Id = "Chomper")] public string UnitName { get; }
        [Inject(Id = "Chomper")] public float ProductionTime { get; }
        

        public GameObject UnitPrefab => _unitPrefab;
        
        
    }
}