using System.Collections.Generic;
using System.Threading.Tasks;
using UnitSystem;
using UnitSystem.MovementSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class UnitCombat
    {
        private readonly Dictionary<UnitType, AUnitAttack> _attacks;
        private readonly IEnumerable<EnemyDetector> _enemyDetectors;
        private readonly UnitMover _unitMover;
        
        public UnitCombat(IEnumerable<AUnitAttack> attacks, IEnumerable<EnemyDetector> enemyDetectors, UnitMover unitMover)
        {
            _attacks = new Dictionary<UnitType, AUnitAttack>();
            foreach (var attack in attacks)
            {
                _attacks.Add(attack.UnitType, attack);
            }
            _enemyDetectors = enemyDetectors;
            foreach (var detector in _enemyDetectors)
            {
                detector.OnDetection += AimOnEnemy;
            }
            _unitMover = unitMover;
        }
        
        private void AimOnEnemy(Unit unit, Unit enemy)
        {
            if(unit == null || unit.Health.IsDead) return;
            if (enemy == null)
            {
                if (!unit.CombatMode) return;
                unit.CombatMode = false;
                _unitMover.UnFollowTargetEnemy(unit);
                return;
            }
            if(enemy != null && (unit.TargetEnemy == null || (unit.TargetEnemy != null && enemy.gameObject.transform != unit.TargetEnemy.transform)))
            {
                if (unit.FollowEnemy)
                {
                    _unitMover.FollowTargetEnemy(unit, enemy.transform, unit.Stats.AttackRange * 0.75f);
                    unit.CombatMode = true;
                }
                unit.TargetEnemy = enemy.transform;
                Attack(unit, enemy);
            }

            if (unit.FollowEnemy)
            {
                unit.transform.LookAt(enemy.transform);
            }
            unit.TargetEnemy = enemy.transform;
        }
        
        private async void Attack(Unit unit, Unit enemy)
        {
            if (enemy == null || unit.TargetEnemy != enemy.transform)
            {
                Debug.Log("No enemy");
                return;
            }
            
            if(enemy.Health.IsDead || unit.Health.IsDead
                                   || Vector3.Distance(unit.transform.position, enemy.transform.position) > unit.Stats.AttackRange)
            {
                if(!unit.IsFollowingEnemy)
                    unit.CombatMode = false;
                Debug.Log("long distance");
                await Task.Delay(200);
                Attack(unit, enemy);
                return;
            }
            unit.CombatMode = true;
            _attacks[unit.UnitType].Attack(unit, enemy);
            Debug.Log("Attack!");
            await Task.Delay((int)(unit.Stats.AttackCooldown*1000));
            
            Attack(unit, enemy);
        }
    }
}