using System;
using System.Linq;
using Core;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    [CreateAssetMenu(fileName = "EnemyCombatData", menuName = "SO/EnemyCombatData")]
    public class EnemyCombatDataSO : ScriptableObject
    {
        [field: SerializeField] public float EnemyDetectionUpdateTime { get; private set; }
    }

    public class MeleeEnemyDetector : IEnemyDetector
    {
        private readonly Unit _unit;
        private readonly LayerMask _enemyLayers;
        private readonly float _radius;
        
        public event Action<Unit> OnEnemyDetection;
        
        public MeleeEnemyDetector(EnemyDetectionUpdater detectionUpdater, 
            Unit unit, LayerMask enemyLayers, float radius)
        {
            _unit = unit;
            _enemyLayers = enemyLayers;
            _radius = radius;
            detectionUpdater.AddDetector(this);
        }
        
        public void DetectEnemy()
        {
            Collider[] enemies = Physics.OverlapSphere(_unit.transform.position, _radius, _enemyLayers);
            
            if(enemies.Length == 0) return;
            Unit unit = enemies.OrderBy(c => (_unit.transform.position - c.transform.position).sqrMagnitude).First()
                .GetComponent<Unit>();
            OnEnemyDetection?.Invoke(unit);
        }
    }
    
    
    public class MeleeAttack: IUnitAttack
    {
        private readonly UpdateTimer _attackCooldownTimer;
        
        
        
        public MeleeAttack(UpdateTimer attackCooldownTimer, float attackCooldown)
        {
            _attackCooldownTimer = attackCooldownTimer;
            _attackCooldownTimer.SetMaxTime(attackCooldown);
        }
        
        public void Attack(Unit unit)
        {
            
        }
    }

    public interface IUnitAttack
    {
        void Attack(Unit unit);
    }
}
