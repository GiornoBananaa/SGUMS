using System;
using System.Collections.Generic;
using UnitGroupingSystem;
using UnityEngine;

namespace SelectionSystem
{
    public class GroupSelection : ISelection<Group>
    {
        private readonly UnitSelection _unitSelection;
        private readonly HashSet<Group> _selectedGroups = new();

        public IEnumerable<Group> Selected => _selectedGroups;
        public int SelectedCount => _selectedGroups.Count;

        public Action<Group> OnGroupSelect;
        public Action<Group> OnGroupDeselect;

        public bool MultiSelection { get; set; }
        
        public GroupSelection(UnitSelection unitSelection)
        {
            _unitSelection = unitSelection;
        }
        
        public void Select(Group group)
        {
            if(!MultiSelection)
                DeselectAll();
            _selectedGroups.Add(group);
            _unitSelection.Select(group.Units);
            OnGroupSelect?.Invoke(group);
        }

        public void Deselect(Group group)
        {
            _selectedGroups.Remove(group);
            OnGroupDeselect?.Invoke(group);
        }
        
        public void DeselectAll()
        {
            foreach (var group in _selectedGroups)
            {
                OnGroupDeselect?.Invoke(group);
            }
            _selectedGroups.Clear();
        }
    }
}