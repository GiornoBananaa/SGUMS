using System;
using UnityEngine;

namespace UnitSystem
{
    [Serializable]
    public class UnitData
    {
        [field: SerializeField] public UnitType UnitType { get; private set; }
        [field: SerializeField] public UnitStats UnitStats { get; private set; }
        [field: SerializeField] public Unit Prefab { get; private set; }
        
    }
    
    [Serializable]
    public struct UnitStats
    {
        public int MaxHP;
        public int Attack;
        public float Speed;
        public float AttackCooldown;
        public float AttackRange;
        public float DetectionRange;
        public float ViewRange;
        
        public UnitStats(int maxHP, int attack, float speed, float attackCooldown, float attackRange, float detectionRange, float viewRange)
        {
            MaxHP = maxHP;
            Attack = attack;
            Speed = speed;
            AttackCooldown = attackCooldown;
            AttackRange = attackRange;
            DetectionRange = detectionRange;
            ViewRange = viewRange;
        }
    }
    
    [Serializable]
    public class ModifiableUnitStats: ICloneable
    {
        [field: SerializeField] public int BaseMaxHP { get; private set; }
        [field: SerializeField] public int BaseAttack { get; private set; }
        [field: SerializeField] public float BaseSpeed { get; private set; }
        [field: SerializeField] public float BaseAttackCooldown { get; private set; }
        [field: SerializeField] public float BaseAttackRange { get; private set; }
        [field: SerializeField] public float BaseDetectionRange { get; private set; }
        [field: SerializeField] public float BaseViewRange { get; private set; }
        
        public int HPModifier { get; set; }
        public int AttackModifier { get; set; }
        public float SpeedModifier { get; set; }
        public float AttackCooldownModifier { get; set; }
        public float AttackRangeModifier { get; set; }
        public float DetectionRangeModifier { get; set; }
        public float ViewRangeModifier { get; set; }
        
        public int MaxHP => BaseMaxHP + HPModifier;
        public int Attack => BaseAttack + AttackModifier;
        public float Speed => BaseSpeed + SpeedModifier;
        public float AttackCooldown => BaseAttackCooldown + AttackCooldownModifier;
        public float AttackRange => BaseAttackRange + AttackRangeModifier;
        public float DetectionRange => BaseDetectionRange + DetectionRangeModifier;
        public float ViewRange => BaseViewRange + ViewRangeModifier;
        
        public UnitStats UnitStats => new UnitStats(MaxHP, Attack, Speed, AttackCooldown, AttackRange, DetectionRange, ViewRange);
        public event Action OnSpeedChange;
        
        public ModifiableUnitStats(int maxHP = 0, int attack = 0, float speed = 0, float attackCooldown = 0, float attackRange = 0, float detectionRange = 0, float viewRange = 0)
        {
            BaseMaxHP = maxHP;
            BaseAttack =  attack;
            BaseSpeed = speed;
            BaseAttackCooldown = attackCooldown;
            BaseAttackRange = attackRange;
            BaseDetectionRange = detectionRange;
            BaseViewRange = viewRange;
        }
        
        public ModifiableUnitStats(UnitStats unitDataUnitStats)
        {
            BaseMaxHP = unitDataUnitStats.MaxHP;
            BaseAttack =  unitDataUnitStats.Attack;
            BaseSpeed = unitDataUnitStats.Speed;
            BaseAttackCooldown = unitDataUnitStats.AttackCooldown;
            BaseAttackRange = unitDataUnitStats.AttackRange;
            BaseDetectionRange = unitDataUnitStats.DetectionRange;
            BaseViewRange = unitDataUnitStats.ViewRange;
        }
        
        public void AddStats(UnitStats otherStats)
        {
            HPModifier += otherStats.MaxHP;
            AttackModifier += otherStats.Attack;
            SpeedModifier += otherStats.Speed;
            AttackCooldownModifier += otherStats.AttackCooldown;
            AttackRangeModifier += otherStats.AttackRange;
            DetectionRangeModifier += otherStats.DetectionRange;
            ViewRangeModifier += otherStats.ViewRange;
            CheckChange(otherStats);
        }
        
        public void SubtractStats(UnitStats otherStats)
        {
            HPModifier -= otherStats.MaxHP;
            AttackModifier -= otherStats.Attack;
            SpeedModifier -= otherStats.Speed;
            AttackCooldownModifier -= otherStats.AttackCooldown;
            AttackRangeModifier -= otherStats.AttackRange;
            DetectionRangeModifier -= otherStats.DetectionRange;
            ViewRangeModifier -= otherStats.ViewRange;
            CheckChange(otherStats);
        }
        
        private void CheckChange(UnitStats otherStats)
        {
            if(otherStats.Speed != 0)
                OnSpeedChange?.Invoke();
        }
        
        public object Clone()
        {
            return new ModifiableUnitStats(UnitStats)
            {
                AttackModifier = AttackModifier,
                AttackCooldownModifier = AttackCooldownModifier,
                AttackRangeModifier = AttackRangeModifier,
                SpeedModifier = SpeedModifier,
                HPModifier = HPModifier,
                DetectionRangeModifier = DetectionRangeModifier,
                ViewRangeModifier = ViewRange,
            };
        }
    }
}