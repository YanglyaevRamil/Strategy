namespace _Strategy._Main.Utils.AsyncExtensions
{
    public interface IAwaitable<T>
    {
        IAwaiter<T> GetAwaiter();
    }
}