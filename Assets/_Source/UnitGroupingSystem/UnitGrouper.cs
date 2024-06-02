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

        public void CreateSquad(IEnumerable<Unit> units)
        {
            Group squad = new Group(units);
            _unitGroupContainer.Add(squad);
            
            GroupEmblemView emblem = _groupEmblemFactory.Create();
            emblem.SetGroup(squad);
        }
    }
}