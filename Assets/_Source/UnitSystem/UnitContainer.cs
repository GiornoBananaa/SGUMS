using System.Collections.Generic;

namespace UnitSystem
{
    public class UnitContainer
    {
        private HashSet<Unit> _units = new();
        public IEnumerable<Unit> PlayerUnits => _units;

        public UnitContainer(IEnumerable<Unit> units)
        {
            _units = new HashSet<Unit>(units);
        }
        
        public void Add(Unit unit)
        {
            _units.Add(unit);
        }
        
        public void Remove(Unit unit)
        {
            _units.Remove(unit);
        }
    }
}