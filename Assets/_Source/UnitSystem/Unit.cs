using System;
using SelectionSystem;
using UnitGroupingSystem;
using UnitSystem.MovementSystem;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace UnitSystem
{
    public enum TeamColor
    {
        Blue,
        Red
    }
    
    public class Unit : MonoBehaviour, ISelectable
    {
        [field: SerializeField] public Projector SelectionProjector { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        [field: SerializeField] public float Radius { get; private set; }
        
        public Health Health { get; private set; }
        public Crowd UnitCrowd { get; set; }
        public Path Path { get; set; }
        public TeamColor TeamColor { get; set; }
        public Vector2 PathOffset { get; set; }
        public Vector3 LastPathPoint { get; set; }
        public int PathPointIndex { get; set; }
        private bool _trackNavigation;
        
        public event Action<Unit> OnDestinationReached;
        
        [Inject]
        public void Construct(Health health)
        {
            Health = health;
        }
        
        private void Awake()
        {
            NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            LastPathPoint = transform.position;
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
