using System;
using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.UserControlSystem.Commands;
using _Strategy._Main.Utils.AssetsInjector;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Model.CommandCreators
{
    
    public sealed class ProduceUnitCommandCreator : CommandCreatorBase<IProduceUnitCommand>
    {

        [Inject] private DiContainer _diContainer;
        [Inject] private AssetsContext _context;

        
        protected override void ClassSpecificCommandCreation(Action<IProduceUnitCommand> creationCallback)
        {

            var produceUnitCommand = _context.Inject(new ProduceUnitCommand());
            _diContainer.Inject(produceUnitCommand);
            creationCallback?.Invoke(_context.Inject(produceUnitCommand));
        }
        
        
    }
}