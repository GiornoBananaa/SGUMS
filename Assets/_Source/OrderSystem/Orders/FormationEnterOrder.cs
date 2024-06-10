using System.Linq;
using SelectionSystem;
using UnitGroupingSystem;

namespace OrderSystem
{
    public class FormationEnterOrder : IOrder
    {
        private readonly GroupSelection _groupSelection;
        
        public Orders OrderType => Orders.FormationEnter;
        public bool Activated => _groupSelection.Selected.Any();

        public FormationEnterOrder(GroupSelection groupSelection)
        {
            _groupSelection = groupSelection;
        }
        
        public void Execute()
        {
            
        }
    }
}