using System.Linq;
using Core;
using UnitGroupingSystem;
using UnitSystem.UnitModifierSystem;
using UnityEngine;

namespace UnitSystem.MovementSystem
{
    public class GroupSpeedEqualizer : IUpdatable
    {
        private readonly UnitGroupContainer _unitGroupContainer;
        private readonly UpdateTimer _updateTimer;
        private UnitStatsModifier _speedModifier;
        
        public GroupSpeedEqualizer(UnitGroupContainer groupContainer, ServiceUpdater updater, 
            UnitGroupContainer unitGroupContainer)
        {
            _speedModifier = new UnitStatsModifier(new ModifiableUnitStats(0,0,0,0,0));
            groupContainer.OnGroupAdd += AddGroup;
            groupContainer.OnGroupRemove += RemoveGroup;
            
            updater.Subscribe(this);
            _unitGroupContainer = unitGroupContainer;
        }
        
        public void Update()
        {
            foreach (var group in _unitGroupContainer.PlayerGroups)
            {
                EqualizeSpeed(group);
            }
        }
        
        private void EqualizeSpeed(Group group)
        {
            float maxTime = 0;
            
            foreach (var unit in group.Units)
            {
                if(group.LaggingUnits.Contains(unit)) continue;
                var navMeshDistance = unit.NavMeshAgent.remainingDistance;
                float distance = navMeshDistance != 0 && !float.IsPositiveInfinity(navMeshDistance)
                    ? unit.NavMeshAgent.remainingDistance
                    : Vector3.Distance(unit.NavMeshAgent.destination, unit.NavMeshAgent.transform.position);
                if (maxTime < distance / unit.Stats.BaseSpeed)
                    maxTime = distance / unit.Stats.BaseSpeed;
            }
            
            foreach (var unit in group.Units)
            {
                _speedModifier.RemoveModifier(unit);
                float distance = unit.NavMeshAgent.remainingDistance != 0 ? unit.NavMeshAgent.remainingDistance
                    : Vector3.Distance(unit.NavMeshAgent.destination, unit.NavMeshAgent.transform.position);
                if(maxTime == 0 || distance == 0 || group.LaggingUnits.Contains(unit)) continue;
                _speedModifier.StatsModifiers.SpeedModifier = distance / maxTime - unit.Stats.BaseSpeed;
                _speedModifier.ApplyModifier(unit);
            }
        }
        
        private void AddGroup(Group group)
        {
            group.OnMoveStart += EqualizeSpeed;
        }
        
        private void RemoveGroup(Group group)
        {
            group.OnMoveStart -= EqualizeSpeed;
            foreach (var unit in group.Units.Where(unit => unit.Modifiers.ContainsKey(_speedModifier)))
            {
                _speedModifier.RemoveModifier(unit);
            }
        }
    }
}