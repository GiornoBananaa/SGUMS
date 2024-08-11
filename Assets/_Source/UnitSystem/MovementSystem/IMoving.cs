namespace UnitSystem.MovementSystem
{
    public interface IMoving
    {
        Path Path { get; set; }
        int PathPointIndex { get; set; }
    }
}