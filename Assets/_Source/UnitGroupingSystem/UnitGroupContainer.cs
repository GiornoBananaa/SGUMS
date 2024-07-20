using System;
using System.Collections.Generic;

namespace UnitGroupingSystem
{
    public class UnitGroupContainer
    {
        private readonly HashSet<Group> _groups;
        public IEnumerable<Group> PlayerGroups => _groups;

        public event Action<Group> OnGroupAdd;
        public event Action<Group> OnGroupRemove;
        
        public UnitGroupContainer() => _groups = new HashSet<Group>(); 
        
        public UnitGroupContainer(IEnumerable<Group> groups)
        {
            _groups = new HashSet<Group>(groups);
        }
        
        public void Add(Group group)
        {
            _groups.Add(group);
            OnGroupAdd?.Invoke(group);
        }
        
        public void Remove(Group group)
        {
            _groups.Remove(group);
            OnGroupRemove?.Invoke(group);
        }
    }
}
