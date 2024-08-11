using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class ShieldAttack: MeleeAttack
    {
        public override UnitType UnitType => UnitType.Shield;
        
        protected override IEnumerable<IAttackModifier> AttackModifiers => new IAttackModifier[]
        {
            new AttackByHeightModifier(-1f, 1f, 0.8f, 1.2f),
            new AttackByRotationModifier(new AttackByRotationModifier.RotationDiapason[]
            {
                new (120, 240, Random.Range(0,1) > 0.5f ? 1 : 0, new [] { UnitType.Shield })
            }),
            new AttackByChanceModifier(new AttackByChanceModifier.AttackChanceByType[]
            {
                new (0.9f, new [] { UnitType.Cavalry }),
            }),
        };
    }
}