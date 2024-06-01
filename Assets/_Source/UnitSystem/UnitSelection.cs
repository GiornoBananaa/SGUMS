using System.Collections.Generic;

namespace UnitSystem
{
    public class UnitSelection
    {
        private readonly HashSet<Unit> _selectedUnits = new();
        public IEnumerable<Unit> SelectedUnits => _selectedUnits;
        
        public void Select(IEnumerable<Unit> units)
        {
            foreach (var unit in units)
            {
                Select(unit);
            }
        }
        
        public void Select(Unit unit)
        {
            _selectedUnits.Add(unit);
            EnableSelectionView(unit);
        }
        
        public void Deselect(Unit unit)
        {
            _selectedUnits.Remove(unit);
            DisableSelectionView(unit);
        }
        
        public void DeselectAll()
        {
            foreach (var selectable in _selectedUnits)
            {
                selectable.SelectionProjector.enabled = false;
            }
            _selectedUnits.Clear();
        }

        private void EnableSelectionView(ISelectable selectable)
        {
            selectable.SelectionProjector.enabled = true;
        }
            
        private void DisableSelectionView(ISelectable selectable)
        {
            selectable.SelectionProjector.enabled = false;
        }
    }
}
