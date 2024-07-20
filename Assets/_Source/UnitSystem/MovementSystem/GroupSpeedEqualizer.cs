using System.Linq;
using UnitGroupingSystem;
using UnityEngine;

namespace UnitSystem.MovementSystem
{
    public class GroupSpeedEqualizer
    {
        private UnitStatsModifier _speedModifier;

        public GroupSpeedEqualizer(UnitGroupContainer groupContainer)
        {
            _speedModifier = new UnitStatsModifier(new ModifiableUnitStats(0,0,0,0,0));
            groupContainer.OnGroupAdd += AddGroup;
            groupContainer.OnGroupRemove += RemoveGroup;
        }
        
        private void EqualizeSpeed(Group group)
        {
            float maxTime = 0;
            
            foreach (var unit in group.Units)
            {
                float distance = Vector3.Distance(unit.NavMeshAgent.destination, unit.NavMeshAgent.transform.position);
                Debug.Log(unit.name + " - " + unit.NavMeshAgent.destination + " ~ " + unit.NavMeshAgent.transform.position + " = " + distance);
                if (maxTime < distance / unit.Stats.BaseSpeed)
                    maxTime = distance / unit.Stats.BaseSpeed;
            }
            foreach (var unit in group.Units)
            {
                _speedModifier.RemoveModifier(unit);
                if(maxTime == 0) continue;
                
                float distance = Vector3.Distance(unit.NavMeshAgent.destination, unit.NavMeshAgent.transform.position);
                _speedModifier.StatsModifiers.SpeedModifier = distance / maxTime - unit.Stats.BaseSpeed;
                _speedModifier.ApplyModifier(unit);
                unit.NavMeshAgent.speed = unit.Stats.Speed;
                Debug.Log("distance: " + distance);
                Debug.Log("unit.Stats.Speed: " + unit.Stats.Speed);
                Debug.Log("--------------------------");
            }
            Debug.Log("maxTime: " + maxTime);
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