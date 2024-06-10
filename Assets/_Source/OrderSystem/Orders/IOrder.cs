namespace OrderSystem
{
    public interface IOrder
    {
        Orders OrderType { get; }
        bool Activated { get; }
        void Execute();
    }
}
