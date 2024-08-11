using SelectionSystem;
using TeamSystem;
using UnitCombatSystem;
using UnitGroupingSystem;
using UnitSystem.MovementSystem;
using UnityEngine;
using Zenject;

namespace UnitSystem.UnitFactories
{
    public class ArcherUnitFactory : UnitFactory
    {
        
        public override UnitType UnitType => UnitType.Archer;
        
        public ArcherUnitFactory(UnitContainer unitContainer, UnitSelection unitSelection,
            TeamsDataSO teamsData, UnitGrouper unitGrouper, UnitsDataSO unitsDataSO) 
            : base(unitContainer, unitSelection, teamsData, unitGrouper, unitsDataSO)
        {
            
        }

        protected override void CreateOtherComponents(Unit unit) { }
    }
}