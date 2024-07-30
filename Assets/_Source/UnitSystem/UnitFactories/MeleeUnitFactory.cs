using SelectionSystem;
using TeamSystem;
using UnitCombatSystem;
using UnitGroupingSystem;
using UnitSystem.MovementSystem;
using UnityEngine;
using Zenject;

namespace UnitSystem.UnitFactories
{
    public class MeleeUnitFactory : UnitFactory
    {
        private EnemyDetectionUpdater _enemyDetectionUpdater;
        private LayerMask _unitsLayers;
        private AUnitAttack _attack;
        
        protected override UnitType UnitType => UnitType.Melee;
        
        public MeleeUnitFactory(EnemyDetectionUpdater enemyDetectionUpdater, EnemyDetectionDataSO detectionData, MeleeAttack meleeAttack,
            DiContainer container, UnitContainer unitContainer, UnitSelection unitSelection, UnitMover unitMover, TeamsDataSO teamsData, UnitGrouper unitGrouper, UnitsDataSO unitsDataSO) 
            : base(container, unitContainer, unitSelection, unitMover, teamsData, unitGrouper,unitsDataSO)
        {
            _attack = meleeAttack;
            _enemyDetectionUpdater = enemyDetectionUpdater;
            _unitsLayers = detectionData.UnitsLayers;
        }

        protected override (IEnemyDetector, AUnitAttack) CreateCombatComponent(Unit unit)
        {
            return (new MeleeEnemyDetector(_enemyDetectionUpdater, unit, _unitsLayers), _attack);
        }

        protected override void CreateOtherComponents(Unit unit) { }
    }
}