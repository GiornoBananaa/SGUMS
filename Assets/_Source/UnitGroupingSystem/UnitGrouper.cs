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
            unit.UnitGroup = group;
        }
        
        public void UngroupUnit(Unit unit)
        {
            if(unit.UnitGroup == null) return;
            unit.UnitGroup.Units.Remove(unit);
            if (unit.UnitGroup.Units.Count == 0)
                unit.UnitGroup.Disband();
            unit.UnitGroup = null;
        }
        
        public void DisbandSquad(Group group)
        {
            foreach (var unit in group.Units)
            {
                unit.UnitGroup = null;
            }
            group.Disband();
        }
    }
}