using _Strategy._Main.Abstractions;
using _Strategy._Main.Abstractions.Commands;


namespace _Strategy._Main.UserControlSystem.Commands
{
    
    public sealed class AttackUnitCommand : IAttackCommand
    {
        
        public IAttackable Target { get; }

        
        public AttackUnitCommand(IAttackable target)
        {
            Target = target;
        }
        
        
    }
}