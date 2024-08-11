using System.Collections.Generic;
using System.Threading.Tasks;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public abstract class AUnitAttack
    {
        protected static readonly int ATTACK_ANIMATOR_TRIGGER = Animator.StringToHash("Attack");
        
        protected abstract IEnumerable<IAttackModifier> AttackModifiers { get; }

        public abstract UnitType UnitType { get; }

        public async void Attack(Unit unit, Unit enemy)
        {
            StartAnimation(unit, enemy);
            await Task.Delay((int)(GetAttackDelay(unit, enemy) * 1000));
            StopAnimation(unit, enemy);
            if(unit == null || enemy == null) return;
            int damage = ModifyAttack(unit, enemy);
            if(enemy == null) return;
            DoDamage(enemy, damage);
            Debug.Log(damage + " | " + enemy.Health.HP);
        }

        protected abstract void StartAnimation(Unit unit, Unit enemy);
        protected abstract void StopAnimation(Unit unit, Unit enemy);
        
        protected abstract float GetAttackDelay(Unit unit, Unit enemy);
        
        private int ModifyAttack(Unit unit, Unit enemy)
        {
            
            int result = unit.Stats.Attack;
            foreach (var modifier in AttackModifiers)
            {
                if (unit == null || enemy == null) 
                    return result;
                result = modifier.ModifyAttack(unit, enemy, result);
                if(result == 0) 
                    break;
            }
            return result;
        }

        private void DoDamage(Unit enemy, int attack) 
            => enemy.Health.HP -= attack;
    }
}