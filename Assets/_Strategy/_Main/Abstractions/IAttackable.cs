namespace _Strategy._Main.Abstractions
{
    
    public interface IAttackable : IHealthHolder
    {
        void ReceiveDamage(float amount);

    }
}