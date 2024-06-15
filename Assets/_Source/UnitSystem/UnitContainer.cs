using System.Collections.Generic;
using Zenject;

namespace UnitSystem
{
    public class UnitContainer
    {
        public HashSet<Unit> AllUnits { get; private set; }
        
        public UnitContainer()
        {
            AllUnits = new HashSet<Unit>();
        }
        
        [Inject]
        public UnitContainer(IEnumerable<Unit> units)
        {
            AllUnits = new HashSet<Unit>(units);
        }
    }
}