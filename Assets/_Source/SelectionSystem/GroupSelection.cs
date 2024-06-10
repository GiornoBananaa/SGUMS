using System.Collections.Generic;
using UnitGroupingSystem;

namespace SelectionSystem
{
    public class GroupSelection : ISelection<Group>
    {
        private readonly UnitSelection _unitSelection;
        private readonly HashSet<Group> _selectedGroups = new();

        public IEnumerable<Group> Selected => _selectedGroups;
        public int SelectedCount => _selectedGroups.Count;
        
        public GroupSelection(UnitSelection unitSelection)
        {
            _unitSelection = unitSelection;
        }
        
        public void Select(Group group)
        {
            _selectedGroups.Add(group);
            _unitSelection.Select(group.Units);
        }
        
        public void DeselectAll()
        {
            _selectedGroups.Clear();
        }
    }
}