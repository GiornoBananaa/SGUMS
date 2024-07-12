using System;
using UnitSystem;

namespace UnitCombatSystem
{
    public interface IEnemyDetector
    {
        event Action<Unit> OnEnemyDetection;
        void DetectEnemy();
    }
}