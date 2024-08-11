using System.Collections.Generic;
using System.Linq;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class AttackByChanceModifier : IAttackModifier
    {
        public struct AttackChanceByType
        {
            public readonly float Chance;
            public readonly HashSet<UnitType> UnitTypes;

            public AttackChanceByType(float chance, IEnumerable<UnitType> unitsType)
            {
                Chance = chance;
                UnitTypes = new HashSet<UnitType>(unitsType);
            }
        }
        
        private readonly IEnumerable<AttackChanceByType> _attackChances;

        public AttackByChanceModifier(IEnumerable<AttackChanceByType> attackChances)
        {
            _attackChances = attackChances;
        }
        
        public int ModifyAttack(Unit unit, Unit enemy, int currentAttack)
        {
            if (_attackChances.Where(attackChance => attackChance.UnitTypes.Contains(enemy.UnitType))
                .Any(attackChance => !TryChance(attackChance.Chance)))
            {
                currentAttack = 0;
            }

            return currentAttack;
        }

        private bool TryChance(float chance) 
            => chance > Random.Range(0,1);
    }
}