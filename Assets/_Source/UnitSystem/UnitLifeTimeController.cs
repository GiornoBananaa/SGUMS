using SelectionSystem;

namespace UnitSystem
{
    public class UnitLifeTimeController
    {
        private readonly Unit _unit;
        private readonly UnitContainer _unitContainer;
        private readonly UnitSelection _unitSelection;

        public UnitLifeTimeController(Unit unit, UnitSelection unitSelection, UnitContainer unitContainer)
        {
            _unit = unit;
            _unitContainer = unitContainer;
            _unitSelection = unitSelection;
            _unit.Health.OnDeath += Die;
            _unit.Health.OnRevive += Revive;
        }
        
        private void Die()
        {
            if (_unitSelection.IsSelected(_unit))
                _unitSelection.Deselect(_unit);
            _unitContainer.AllUnits.Remove(_unit);
            if(_unit.UnitCrowd != null)
                _unit.UnitCrowd.Units.Remove(_unit);
            if(_unit.Path != null)
                _unit.Path.RemoveUnit(_unit);
            UnityEngine.Object.Destroy(_unit.gameObject);
        }
        
        private void Revive(int hp)
        {
            
        }
    }
}