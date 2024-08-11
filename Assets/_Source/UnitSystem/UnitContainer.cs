using System.Collections.Generic;
using Zenject;

namespace UnitSystem
{
    public class UnitContainer
    {
        private HashSet<Unit> _allUnits;
        public IEnumerable<Unit> AllUnits => _allUnits;
        
        public UnitContainer()
        {
            _allUnits = new HashSet<Unit>();
        }
        
        [Inject]
        public UnitContainer(IEnumerable<Unit> units)
        {
            _allUnits = new HashSet<Unit>(units);
        }

        public void Add(Unit unit)
        {
            _allUnits.Add(unit);
        }
        
        public void Remove(Unit unit)
        {
            _allUnits.Remove(unit);
        }
    }
}