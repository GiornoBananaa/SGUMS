using System;
using System.Collections.Generic;
using System.Linq;
using SelectionSystem;
using UnitGroupingSystem;
using UnitSystem;
using UnitSystem.MovementSystem;

namespace OrderSystem
{
    public class PathCancelOrder : IOrder
    {
        //private readonly UnitSelection _unitSelection;
        //private readonly GroupSelection _groupSelection;
        private readonly IEnumerable<ISelection<IMoving>> _selections;
        private readonly Dictionary<Type,IPathMover<IMoving>> _movers;
        private readonly GroupMover _groupMover;
        //private readonly UnitMover _unitMover;
        private readonly PathDrawer _pathDrawer;

        public Orders OrderType => Orders.PathCancel;
        public bool Activated
        {
            get
            {
                /*
                return _groupSelection.Selected.Any(group => group.Path != null) 
                       || _unitSelection.Selected.Any(unit => unit.UnitCrowd is not Group && unit.Path != null);*/
                foreach (var selection in _selections)
                {
                    foreach (var selected in selection.Selected)
                    {
                        if (selected.Path != null)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public PathCancelOrder(IEnumerable<ISelection<IMoving>> selections, IEnumerable<IPathMover<IMoving>> movers)
        {
            _selections = selections;
            _movers = new Dictionary<Type, IPathMover<IMoving>>();
            foreach (var mover in movers)
            {
                _movers.Add(mover.GetType().GetGenericArguments()[0],mover);
            }
            //_unitSelection = unitSelection;
            //_groupSelection = groupSelection;
        }
        
        public void Execute()
        {
            foreach (var selection in _selections)
            {
                foreach (var selected in selection.Selected)
                {
                    if (selected.Path != null)
                    {
                        _movers[selected.GetType()].StopOnPath(selected);
                    }
                }
            }
        }
    }
}