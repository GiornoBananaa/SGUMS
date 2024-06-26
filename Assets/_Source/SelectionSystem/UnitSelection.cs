using System;
using System.Collections.Generic;
using UnitSystem;

namespace SelectionSystem
{
    public class UnitSelection : ISelection<Unit>
    {
        private int _selectionProcessesCount;
        private readonly HashSet<Unit> _selectedUnits = new();
        
        public IEnumerable<Unit> Selected => _selectedUnits;
        public int SelectedCount => _selectedUnits.Count;
        
        public Action OnSelectionChanged;
        public Action<Unit> OnUnitSelect;
        public Action<Unit> OnUnitDeselect;
        
        public bool MultiSelection { get; set; }
        
        public void Select(IEnumerable<Unit> units)
        {
            if(!MultiSelection)
                DeselectAll();
            _selectionProcessesCount += 1;
            foreach (var unit in units)
            {
                Select(unit);
            }
            _selectionProcessesCount -= 1;
            OnSelectionChanged?.Invoke();
        }
        
        public void Select(Unit unit)
        {
            if(!MultiSelection && _selectionProcessesCount == 0)
                DeselectAll();
            _selectedUnits.Add(unit);
            EnableSelectionView(unit);
            OnUnitSelect?.Invoke(unit);
            if(_selectionProcessesCount == 0)
                OnSelectionChanged?.Invoke();
        }
        
        public void Deselect(Unit unit)
        {
            _selectedUnits.Remove(unit);
            DisableSelectionView(unit);
            OnUnitDeselect?.Invoke(unit);
            if(_selectionProcessesCount == 0)
                OnSelectionChanged?.Invoke();
        }
        
        public void DeselectAll()
        {
            foreach (var selectable in _selectedUnits)
            {
                DisableSelectionView(selectable);
                OnUnitDeselect?.Invoke(selectable);
            }
            _selectedUnits.Clear();
            OnSelectionChanged?.Invoke();
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
