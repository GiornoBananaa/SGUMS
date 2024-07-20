using System.Collections.Generic;
using System.Linq;
using Core;

namespace UnitCombatSystem
{
    public class EnemyDetectionUpdater
    {
        private readonly UpdateTimer _timer;
        private readonly HashSet<IEnemyDetector> _enemyDetectors;
        
        public EnemyDetectionUpdater(UpdateTimer timer, EnemyDetectionDataSO enemyDetectionData)
        {
            _timer = timer;
            _enemyDetectors = new HashSet<IEnemyDetector>();
            _timer.SetMaxTime(enemyDetectionData.EnemyDetectionUpdateTime);
            _timer.OnTimerEnd += UpdateDetectors;
            _timer.Restart();
        }

        public void AddDetector(IEnemyDetector detector)
        {
            _enemyDetectors.Add(detector);
        }
        
        public void RemoveDetector(IEnemyDetector detector)
        {
            _enemyDetectors.Remove(detector);
        }
        
        private void UpdateDetectors()
        {
            foreach (var detector in _enemyDetectors.ToList())
            {
                detector.DetectEnemy();
            }
            _timer.Restart();
        }
    }
}