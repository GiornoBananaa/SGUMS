using UnitSystem;

namespace UnitCombatSystem
{
    public interface IUnitAttack
    {
        void Attack(Unit unit, Unit enemy);
    }
}