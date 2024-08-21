using System;
using System.Collections.Generic;
using FogOfWarSystem;
using SelectionSystem;
using TeamSystem;
using UnitGroupingSystem;
using UnitSystem.MovementSystem;
using UnitSystem.UnitModifierSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

namespace UnitSystem
{
    public class Unit : MonoBehaviour, ISelectable, IMoving
    {
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public FieldOfView FieldOfView { get; private set; }
        [field: SerializeField] public DecalProjector SelectionProjector { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ParticleSystem AttackEffect { get; private set; }
        
        private bool _trackNavigation;
        private bool _combatMode;
        
        public Dictionary<IUnitModifier, UnitStats> Modifiers { get; private set; } = new();
        public Health Health { get; private set; }
        public ModifiableUnitStats Stats { get; private set; }
        public Crowd UnitCrowd { get; set; }
        public Path Path { get; set; }
        public Transform TargetEnemy { get; set; }
        public Vector2 TargetOffset { get; set; }
        public Vector2 PathOffset { get; set; }
        public TeamColor TeamColor { get; set; }
        public UnitType UnitType { get; set; }
        public int PathPointIndex { get; set; }
        public bool IsMoving { get; private set; }
        public bool IsFollowingEnemy { get; set; }
        public bool FollowEnemy { get; set; } = true;
        public int TerrainUnderUnit { get; set; }
        public bool CombatMode
        {
            get => _combatMode;
            set
            {
                _combatMode = value;
                NavMeshAgent.obstacleAvoidanceType = value ? 
                    ObstacleAvoidanceType.MedQualityObstacleAvoidance
                    :ObstacleAvoidanceType.NoObstacleAvoidance;
            }
        }
        
        public event Action<Unit> OnDestinationReached;
        public event Action<Unit> OnPathEnd;
        
        public void Construct(Health health, ModifiableUnitStats stats, TeamColor teamColor, UnitType unitType)
        {
            Health = health;
            Stats = stats;
            TeamColor = teamColor;
            UnitType = unitType;
            FieldOfView.SetSize(stats.ViewRange*2);
            Stats.OnSpeedChange += OnSpeedChanged;
        }
        
        private void Awake()
        {
            CombatMode = false;
        }
        
        private void Update()
        {
            if(!_trackNavigation) return;
            
            if (!NavMeshAgent.pathPending && !NavMeshAgent.hasPath)
            {
                EndNavigationTracking();
                OnDestinationReached?.Invoke(this);
                if(Path == null || PathPointIndex >= Path.PathPoints.Count)
                    OnPathEnd?.Invoke(this);
            }
        }
        
        public void StartNavigationTracking()
        {
            _trackNavigation = true;
            IsMoving = true;
        }
        
        public void EndNavigationTracking()
        {
            _trackNavigation = false;
            IsMoving = false;
        }
        
        private void OnSpeedChanged()
        {
            NavMeshAgent.speed = Stats.Speed;
        }
        
    }
}
