﻿using System.Linq;
using SelectionSystem;
using UnitGroupingSystem;

namespace OrderSystem
{
    public class SquadDisbandOrder : IOrder
    {
        private readonly UnitGrouper _unitGrouper;
        private readonly GroupSelection _groupSelection;
        
        public Orders OrderType => Orders.SquadDisband;
        public bool Activated => _groupSelection.Selected.Any();

        public SquadDisbandOrder(GroupSelection groupSelection, UnitGrouper unitGrouper)
        {
            _groupSelection = groupSelection;
            _unitGrouper = unitGrouper;
        }
        
        public void Execute()
        {
            foreach (var group in _groupSelection.Selected)
            {
                _unitGrouper.DisbandSquad(group);
            }
        }
    }
}