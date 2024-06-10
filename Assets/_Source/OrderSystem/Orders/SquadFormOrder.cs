using System.Linq;
using SelectionSystem;
using UnitGroupingSystem;

namespace OrderSystem
{
    public class SquadFormOrder : IOrder
    {
        private readonly UnitSelection _unitSelection;
        private readonly UnitGrouper _unitGrouper;
        private readonly GroupSelection _groupSelection;
        
        public Orders OrderType => Orders.SquadForm;
        public bool Activated => _unitSelection.Selected.Count() > 1 
                                 && !(_groupSelection.SelectedCount == 1 && _unitSelection.SelectedCount == _groupSelection.Selected.First().Units.Count);

        public SquadFormOrder(UnitSelection unitSelection, GroupSelection groupSelection, UnitGrouper unitGrouper)
        {
            _unitSelection = unitSelection;
            _unitGrouper = unitGrouper;
            _groupSelection = groupSelection;
        }
        
        public void Execute()
        {
            Group squad = _unitGrouper.CreateSquad(_unitSelection.Selected);
            _groupSelection.Select(squad);
        }
    }
}