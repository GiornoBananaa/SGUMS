using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class AttackByRotationModifier : IAttackModifier
    {
        public struct RotationDiapason
        {
            public float From;
            public float To;
            public float Modifier;
            public HashSet<UnitType> UnitTypes;

            public RotationDiapason(float from, float to, float modifier, IEnumerable<UnitType> unitsType)
            {
                From = from;
                To = to;
                Modifier = modifier;
                UnitTypes = new HashSet<UnitType>(unitsType);
            }
        }
        
        private readonly IEnumerable<RotationDiapason> _rotationDiapasons;

        public AttackByRotationModifier(IEnumerable<RotationDiapason> rotationDiapasons)
        {
            _rotationDiapasons = rotationDiapasons;
        }
        
        public int ModifyAttack(Unit unit, Unit enemy, int currentAttack)
        {
            Vector3 relativeRotation = (Quaternion.Inverse(unit.transform.rotation) * enemy.transform.rotation).eulerAngles;
            foreach (var diapason in _rotationDiapasons)
            {
                if (!diapason.UnitTypes.Contains(enemy.UnitType)) continue;
                if (AngleInDiapason(relativeRotation.y, diapason.From, diapason.To))
                {
                    currentAttack = (int)(currentAttack * diapason.Modifier);
                    break;
                }
            }
            return currentAttack;
        }

        private bool AngleInDiapason(float angle, float minAngle, float maxAngle)
        {
            if (minAngle < maxAngle)
            {
                if (angle < minAngle && minAngle > maxAngle)
                    return false;
            }
            else
            {
                if (angle > minAngle && minAngle < maxAngle)
                    return false;
            }
            return true;
        }
    }
}