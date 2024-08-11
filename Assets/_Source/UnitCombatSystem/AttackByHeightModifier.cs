using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class AttackByHeightModifier : IAttackModifier
    {
        private readonly float _maxHeightDifference;
        private readonly float _minHeightDifference;
        private readonly float _maxMultiplier;
        private readonly float _minMultiplier;

        public AttackByHeightModifier(float minHeight, float maxHeight, float minMultiplier, float maxMultiplier)
        {
            _minHeightDifference = minHeight;
            _maxHeightDifference = maxHeight;
            _minMultiplier = minMultiplier;
            _maxMultiplier = maxMultiplier;
        }
        
        public int ModifyAttack(Unit unit, Unit enemy, int currentAttack)
        {
            float heightDifference = (unit.transform.position.y - enemy.transform.position.y);
            heightDifference = heightDifference > _maxHeightDifference ? _maxHeightDifference : heightDifference < _minHeightDifference ? _minHeightDifference : heightDifference;
            float modifier = Mathf.Lerp(_minMultiplier, _maxMultiplier, Mathf.InverseLerp(_minHeightDifference,_maxHeightDifference,heightDifference));
            return (int)(modifier * currentAttack);
        }
    }
}