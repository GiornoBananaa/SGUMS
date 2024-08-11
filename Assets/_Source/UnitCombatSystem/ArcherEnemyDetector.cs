using System.Linq;
using Core;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class ArcherEnemyDetector : EnemyDetector
    {
        public ArcherEnemyDetector(UpdateTimer updateTimer, UnitContainer unitContainer, EnemyDetectionDataSO enemyDetectionData) : base(unitContainer, updateTimer, enemyDetectionData)
        { }

        protected override Unit ChooseEnemy(Unit unit, Collider[] enemies)
        {
            Unit enemy = enemies.OrderBy(c => (unit.transform.position - c.transform.position).sqrMagnitude).First()
                .GetComponent<Unit>();
            
            return enemy;
        }
    }
}