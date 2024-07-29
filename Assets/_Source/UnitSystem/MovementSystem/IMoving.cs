using UnitSystem.MovementSystem;

namespace UnitGroupingSystem
{
    public interface IMoving
    {
        Path Path { get; set; }
        int PathPointIndex { get; set; }
    }
}