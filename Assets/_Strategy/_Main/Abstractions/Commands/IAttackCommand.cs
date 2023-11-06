namespace _Strategy._Main.Abstractions.Commands
{
    
    public interface IAttackCommand : ICommand
    {

        public IAttackable Target { get; }
        

    }
}