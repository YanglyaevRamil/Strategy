using _Strategy._Main.Abstractions;
using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.UserControlSystem.Commands;
using _Strategy._Main.Utils.AssetsInjector;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.Model.CommandCreators
{
    
    public sealed class AttackUnitCommandCreator : CancellableCommandCreatorBase<IAttackCommand, IAttackable>
    {

        [Inject] private AssetsContext _context;
        [Inject] private AttackableValue _groundClicks;


        protected override IAttackCommand CreateCommand(IAttackable argument) => new AttackUnitCommand(argument);
    }
}