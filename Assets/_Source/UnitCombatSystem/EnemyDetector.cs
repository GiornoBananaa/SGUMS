using System;
using Core;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public abstract class EnemyDetector
    {
        private readonly LayerMask _unitsLayers;
        private readonly UpdateTimer _timer;
        private readonly UnitContainer _unitContainer;
        
        public event Action<Unit, Unit> OnDetection;
        
        public EnemyDetector(UnitContainer unitContainer, UpdateTimer timer,
            EnemyDetectionDataSO enemyDetectionData)
        {
            _timer = timer;
            _timer.SetMaxTime(enemyDetectionData.EnemyDetectionUpdateTime);
            _timer.OnTimerEnd += Update;
            _timer.Restart();
            _unitsLayers = enemyDetectionData.UnitsLayers;
            _unitContainer = unitContainer;
        }
        
        public void Update()
        {
            foreach (var unit in _unitContainer.AllUnits)
            {
                DetectEnemies(unit);
            }
            _timer.Restart();
        }
        
        private void DetectEnemies(Unit unit)
        {
            LayerMask enemyLayers = _unitsLayers & ~(1 << unit.gameObject.layer);
            if(unit == null)
            {
                return;
            }
            Collider[] enemies = Physics.OverlapSphere(unit.transform.position, unit.Stats.DetectionRange, enemyLayers);
            
            if(enemies.Length == 0)
            {
                OnDetection?.Invoke(unit, null);
                return;
            }
            OnDetection?.Invoke(unit, ChooseEnemy(unit, enemies));
        }

        protected abstract Unit ChooseEnemy(Unit unit, Collider[] enemies);
    }
}