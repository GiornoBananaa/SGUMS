using SelectionSystem;
using TeamSystem;
using UnitCombatSystem;
using UnitGroupingSystem;
using UnitSystem.MovementSystem;
using Zenject;

namespace UnitSystem.UnitFactories
{
    public class SpearUnitFactory : UnitFactory
    {
        public override UnitType UnitType => UnitType.Spear;
        public SpearUnitFactory(UnitContainer unitContainer, UnitSelection unitSelection,
            TeamsDataSO teamsData, UnitGrouper unitGrouper, UnitsDataSO unitsDataSO) 
            : base(unitContainer, unitSelection, teamsData, unitGrouper, unitsDataSO)
        { }
        
        protected override void CreateOtherComponents(Unit unit) { }
    }
}