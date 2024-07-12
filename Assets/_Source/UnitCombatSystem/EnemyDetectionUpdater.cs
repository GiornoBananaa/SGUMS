using System.Collections.Generic;
using Core;

namespace UnitCombatSystem
{
    public class EnemyDetectionUpdater
    {
        private readonly UpdateTimer _timer;
        private readonly HashSet<IEnemyDetector> _enemyDetectors;
        
        public EnemyDetectionUpdater(UpdateTimer timer, EnemyCombatDataSO enemyCombatData)
        {
            _timer = timer;
            _enemyDetectors = new HashSet<IEnemyDetector>();
            _timer.SetMaxTime(enemyCombatData.EnemyDetectionUpdateTime);
            _timer.OnTimerEnd += UpdateDetectors;
            _timer.Restart();
        }

        public void AddDetector(IEnemyDetector detector)
        {
            _enemyDetectors.Add(detector);
        }

        private void UpdateDetectors()
        {
            foreach (var detector in _enemyDetectors)
            {
                detector.DetectEnemy();
            }
        }
    }
}