using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class SpearAttack: MeleeAttack
    {
        public override UnitType UnitType => UnitType.Spear;
        
        protected override IEnumerable<IAttackModifier> AttackModifiers => new IAttackModifier[]
        {
            new AttackByHeightModifier(-2f, 2f, 0.7f, 1.3f),
            new AttackByRotationModifier(new AttackByRotationModifier.RotationDiapason[]
            {
                new (120, 240, Random.Range(0,1) > 0.6f ? 1 : 0, new [] { UnitType.Shield })
            }),
            new AttackByChanceModifier(new AttackByChanceModifier.AttackChanceByType[]
            {
                new (0.9f, new [] { UnitType.Cavalry }),
            }),
        };
    }
}