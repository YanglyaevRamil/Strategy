using _Strategy._Main.Abstractions;
using _Strategy._Main.Abstractions.Commands;


namespace _Strategy._Main.Core
{
    
    public sealed class AutoAttackUnitCommand : IAttackCommand
    {
        
        public IAttackable Target { get; }

        
        public AutoAttackUnitCommand(IAttackable target)
        {
            Target = target;
        }
        
        
    }
}