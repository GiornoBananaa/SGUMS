using UnitSystem;

namespace UnitCombatSystem
{
    public interface IAttackModifier
    {
        int ModifyAttack(Unit unit, Unit enemy, int currentAttack);
    }
}