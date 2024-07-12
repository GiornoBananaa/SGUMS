using System.Collections.Generic;
using Unit = UnitSystem.Unit;

namespace UnitGroupingSystem
{
    public class UnitGrouper
    {
        private UnitGroupContainer _unitGroupContainer;
        private GroupEmblemFactory _groupEmblemFactory;
        
        public UnitGrouper(UnitGroupContainer unitGroupContainer, GroupEmblemFactory groupEmblemFactory)
        {
            _unitGroupContainer = unitGroupContainer;
            _groupEmblemFactory = groupEmblemFactory;
        }

        public Group CreateSquad(IEnumerable<Unit> units)
        {
            Group squad = new Group();
            foreach (var unit in units)
            {
                UngroupUnit(unit);
                GroupUnit(unit, squad);
            }
            
            _unitGroupContainer.Add(squad);
            
            GroupEmblemView emblem = _groupEmblemFactory.Create();
            emblem.SetGroup(squad);
            return squad;
        }
        
        public void GroupUnit(Unit unit, Group group)
        {
            group.Units.Add(unit);
            unit.UnitCrowd = group;
        }
        
        public void UngroupUnit(Unit unit)
        {
            if(unit.UnitCrowd == null) return;
            unit.UnitCrowd.Units.Remove(unit);
            if (unit.UnitCrowd.Units.Count == 0 && unit.UnitCrowd is Group group)
                group.Disband();
            unit.UnitCrowd = null;
        }
        
        public void DisbandSquad(Group group)
        {
            foreach (var unit in group.Units)
            {
                unit.UnitCrowd = null;
            }
            group.Disband();
        }
    }
}