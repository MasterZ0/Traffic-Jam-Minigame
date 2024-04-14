namespace Marmalade.TheGameOfLife.TrafficJam
{
    public interface ICashHandler
    {
        void AddCash(int amount);
        int RemoveCash(int amount);
    }
}
