using Core;
using UnitSystem;
using UnitSystem.MovementSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class UnitCombat
    {
        private readonly IUnitAttack _attack;
        private readonly UpdateTimer _attackCooldownTimer;
        private readonly UpdateTimer _attackRangeTimer;
        private readonly IEnemyDetector _enemyDetector;
        private readonly UnitMover _unitMover;
        private readonly Unit _unit;
        private Unit _enemy;
        private bool _seesEnemy;
        private bool _isFighting;
        
        public UnitCombat(Unit unit, UpdateTimer attackCooldownTimer, UpdateTimer attackRangeTimer, IUnitAttack attack, 
            IEnemyDetector enemyDetector, UnitMover unitMover)
        {
            _unit = unit;
            _attack = attack;
            _enemyDetector = enemyDetector;
            _unitMover = unitMover;
            _attackCooldownTimer = attackCooldownTimer;
            _attackRangeTimer = attackRangeTimer;
            _attackCooldownTimer.SetMaxTime(0);
            _attackRangeTimer.SetMaxTime(0.1f);
            _enemyDetector.OnEnemyDetection += AimOnEnemy;
            _attackCooldownTimer.OnTimerEnd += BecomeReadyToAttack;
        }

        private void BecomeReadyToAttack()
        {
            _attackRangeTimer.OnTimerEnd += Attack;
            _attackRangeTimer.Restart();
        }
        
        private void AimOnEnemy(Unit enemy)
        {
            if(_unit == null) return;
            if (enemy == null && _seesEnemy)
            {
                _seesEnemy = false;
                _unitMover.UnFollowTarget(_unit);
                _attackCooldownTimer.Stop();
            }
            else if(enemy != null)
            {
                _seesEnemy = true;
                _unitMover.FollowTarget(_unit, enemy.transform, enemy.Radius + _unit.Radius * 2);
                _attackCooldownTimer.SetMaxTime(_unit.Stats.AttackCooldown);
                _attackCooldownTimer.Restart();
            }
            _enemy = enemy;
        }
        
        private void Attack()
        {
            if (_enemy == null)
            {
                _attackRangeTimer.OnTimerEnd -= Attack;
                return;
            }
            if(_enemy.Health.IsDead || _unit.Health.IsDead
               || Vector3.Distance(_unit.transform.position, _enemy.transform.position) > _unit.Stats.AttackRange)
            {
                _attackRangeTimer.Restart();
                return;
            }
            _attackRangeTimer.OnTimerEnd -= Attack;
            _attack.Attack(_unit, _enemy);
        }
    }
}