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
        [field: SerializeField] public float Radius { get; private set; }
        public Group UnitGroup { get; set; }
        public Path Path { get; set; }
        public Vector2 PathOffset { get; set; }
        public int PathPointIndex { get; set; }
        private bool _trackNavigation;
        
        public event Action<Unit> OnDestinationReached;

        private void Awake()
        {
            NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        public void StartNavigationTracking()
        {
            _trackNavigation = true;
        }

        private void Update()
        {
            if(!_trackNavigation) return;
            
            
            if (!NavMeshAgent.pathPending && !NavMeshAgent.hasPath)
            {
                EndNavigationTracking();
                OnDestinationReached?.Invoke(this);
            }
        }
        
        private void EndNavigationTracking()
        {
            _trackNavigation = false;
        }
    }
}
