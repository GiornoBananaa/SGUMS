using System.Collections.Generic;

namespace UnitGroupingSystem
{
    public class UnitGroupContainer
    {
        private readonly HashSet<Group> _groups;
        public IEnumerable<Group> PlayerGroups => _groups;
        
        public UnitGroupContainer() => _groups = new HashSet<Group>(); 
        
        public UnitGroupContainer(IEnumerable<Group> groups)
        {
            _groups = new HashSet<Group>(groups);
        }
        
        public void Add(Group group)
        {
            _groups.Add(group);
        }
        
        public void Remove(Group group)
        {
            _groups.Remove(group);
        }
    }
}
