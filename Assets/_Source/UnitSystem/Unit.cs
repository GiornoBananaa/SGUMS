using System;
using SelectionSystem;
using UnitGroupingSystem;
using UnitSystem.MovementSystem;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem
{
    public class Unit : MonoBehaviour,ISelectable
    {
        [field: SerializeField] public Projector SelectionProjector { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        public Group UnitGroup { get; set; }
        public Path Path { get; set; }
        public int PathPointIndex { get; set; }
        private bool _trackNavigation;
        
        public event Action<Unit> OnDestinationReached;
        
        public void StartNavigationTracking()
        {
            _trackNavigation = true;
        }

        private void Update()
        {
            if(!_trackNavigation) return;
            
            if (NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                OnDestinationReached?.Invoke(this);
            }

            EndNavigationTracking();
        }
        
        private void EndNavigationTracking()
        {
            _trackNavigation = false;
        }
    }
}
