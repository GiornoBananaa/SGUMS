using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitSystem;

namespace UnitCombatSystem
{
    public abstract class AUnitAttack
    {
        protected abstract IEnumerable<IAttackModifier> AttackModifiers { get; }
        
        public async void Attack(Unit unit, Unit enemy)
        {
            await Task.Delay((int)(GetAttackDelay(unit, enemy) * 1000));
            DoDamage(enemy, ModifyAttack(unit, enemy));
        }

        protected abstract void StartAnimation(Unit unit, Unit enemy);
        
        protected abstract float GetAttackDelay(Unit unit, Unit enemy);
        
        private int ModifyAttack(Unit unit, Unit enemy)
        {
            return AttackModifiers.Aggregate(unit.Stats.Attack, 
                (current, modifier) => modifier.ModifyAttack(unit, enemy, current));
        }
        
        private void DoDamage(Unit enemy, int attack)
        {
            enemy.Health.HP -= attack;
        }
    }
}