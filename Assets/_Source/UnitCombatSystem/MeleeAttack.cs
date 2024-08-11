using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public abstract class MeleeAttack: AUnitAttack
    {
        protected override void StartAnimation(Unit unit, Unit enemy)
        {
            unit.Animator.SetTrigger(ATTACK_ANIMATOR_TRIGGER);
        }

        protected override void StopAnimation(Unit unit, Unit enemy) { }

        protected override float GetAttackDelay(Unit unit, Unit enemy)
        {
            float distance = Vector3.Distance(unit.transform.position, enemy.transform.position);
            distance = distance > unit.Stats.AttackRange ? unit.Stats.AttackRange : distance;
            return distance * 0.5f;
        }
    }
}