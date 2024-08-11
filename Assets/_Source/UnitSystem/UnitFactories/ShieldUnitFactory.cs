using SelectionSystem;
using TeamSystem;
using UnitCombatSystem;
using UnitGroupingSystem;
using UnitSystem.MovementSystem;
using Zenject;

namespace UnitSystem.UnitFactories
{
    public class ShieldUnitFactory : UnitFactory
    {
        public override UnitType UnitType => UnitType.Shield;

        public ShieldUnitFactory(UnitContainer unitContainer, UnitSelection unitSelection,
            TeamsDataSO teamsData, UnitGrouper unitGrouper, UnitsDataSO unitsDataSO) 
            : base(unitContainer, unitSelection, teamsData, unitGrouper, unitsDataSO)
        {
        }
        
        protected override void CreateOtherComponents(Unit unit) { }
    }
}