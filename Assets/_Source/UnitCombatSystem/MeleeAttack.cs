using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class MeleeAttack: AUnitAttack
    {
        protected override IEnumerable<IAttackModifier> AttackModifiers => new IAttackModifier[]
        {
            
        };

        protected override void StartAnimation(Unit unit, Unit enemy)
        {
            
        }

        protected override float GetAttackDelay(Unit unit, Unit enemy)
        {
            float distance = Vector3.Distance(unit.transform.position, enemy.transform.position);
            distance = distance > unit.Stats.AttackRange ? unit.Stats.AttackRange : distance;
            return distance * 0.5f;
        }
    }
}