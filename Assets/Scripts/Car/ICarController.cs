namespace Marmalade.TheGameOfLife.Car
{
    public interface ICarController
    {
        float Movement { get; }
        float Direction { get; }
        bool Brake { get; }
        bool Active { get; }
    }
}
