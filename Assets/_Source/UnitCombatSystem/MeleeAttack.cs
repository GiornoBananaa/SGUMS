using UnitSystem;

namespace UnitCombatSystem
{
    public class MeleeAttack: IUnitAttack
    {
        public void Attack(Unit unit, Unit enemy)
        {
            enemy.Health.HP -= unit.Stats.Attack;
        }
    }
}