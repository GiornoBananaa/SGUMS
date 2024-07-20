using System;
using System.Collections.Generic;
using SelectionSystem;
using TeamSystem;
using UnitGroupingSystem;
using UnitSystem.MovementSystem;
using UnityEngine;
using UnityEngine.AI;

namespace UnitSystem
{
    public class Unit : MonoBehaviour, ISelectable
    {
        [field: SerializeField] public Projector SelectionProjector { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        [field: SerializeField] public float Radius { get; private set; }
        
        private bool _trackNavigation;
        private bool _combatMode;

        public Dictionary<IUnitModifier, UnitStats> Modifiers { get; private set; } = new();
        public Health Health { get; private set; }
        public ModifiableUnitStats Stats { get; set; }
        public Crowd UnitCrowd { get; set; }
        public Path Path { get; set; }
        public Transform Target { get; set; }
        public Vector2 TargetOffset { get; set; }
        public TeamColor TeamColor { get; set; }
        public Vector2 PathOffset { get; set; }
        public Vector3 LastPathPoint { get; set; }
        public int PathPointIndex { get; set; }
        public bool CombatMode
        {
            get => _combatMode;
            set
            {
                _combatMode = value;
                NavMeshAgent.obstacleAvoidanceType = value ? 
                    ObstacleAvoidanceType.MedQualityObstacleAvoidance
                    :ObstacleAvoidanceType.LowQualityObstacleAvoidance;
            }
        }
        
        public event Action<Unit> OnDestinationReached;
        
        public void Construct(Health health, TeamColor teamColor, ModifiableUnitStats stats)
        {
            Health = health;
            Stats = stats;
            TeamColor = teamColor;
        }
        
        private void Awake()
        {
            LastPathPoint = transform.position;
            CombatMode = false;
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
