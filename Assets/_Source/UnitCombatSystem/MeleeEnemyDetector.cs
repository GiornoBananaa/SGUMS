using System;
using System.Linq;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class MeleeEnemyDetector : IEnemyDetector
    {
        private readonly Unit _unit;
        private readonly EnemyDetectionUpdater _detectionUpdater;
        private readonly LayerMask _enemyLayers;
        
        public event Action<Unit> OnEnemyDetection;
        
        public MeleeEnemyDetector(EnemyDetectionUpdater detectionUpdater, 
            Unit unit, LayerMask enemyLayers)
        {
            _unit = unit;
            _enemyLayers = enemyLayers & ~(1 << unit.gameObject.layer);
            _detectionUpdater = detectionUpdater;
            _detectionUpdater.AddDetector(this);
        }
        
        public void DetectEnemy()
        {
            if(_unit == null)
            {
                _detectionUpdater.RemoveDetector(this);
                return;
            }
            Collider[] enemies = Physics.OverlapSphere(_unit.transform.position, _unit.Stats.AttackRange, _enemyLayers);
            
            if(enemies.Length == 0)
            {
                OnEnemyDetection?.Invoke(null);
                return;
            }
            Unit unit = enemies.OrderBy(c => (_unit.transform.position - c.transform.position).sqrMagnitude).First()
                .GetComponent<Unit>();
            OnEnemyDetection?.Invoke(unit);
        }
    }
}
