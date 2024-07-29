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

        public UnitStats(int maxHP, int attack, float speed, float attackCooldown, float attackRange)
        {
            MaxHP = maxHP;
            Attack = attack;
            Speed = speed;
            AttackCooldown = attackCooldown;
            AttackRange = attackRange;
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
        
        public int HPModifier { get; set; }
        public int AttackModifier { get; set; }
        public float SpeedModifier { get; set; }
        public float AttackCooldownModifier { get; set; }
        public float AttackRangeModifier { get; set; }
        
        public int MaxHP => BaseMaxHP + HPModifier;
        public int Attack => BaseAttack + AttackModifier;
        public float Speed => BaseSpeed + SpeedModifier;
        public float AttackCooldown => BaseAttackCooldown + AttackCooldownModifier;
        public float AttackRange => BaseAttackRange + AttackRangeModifier;

        public UnitStats UnitStats => new UnitStats(MaxHP, Attack, Speed, AttackCooldown, AttackRange);
        public event Action OnSpeedChange;

        public ModifiableUnitStats(int maxHP, int attack, float speed, float attackCooldown, float attackRange)
        {
            BaseMaxHP = maxHP;
            BaseAttack =  attack;
            BaseSpeed = speed;
            BaseAttackCooldown = attackCooldown;
            BaseAttackRange = attackRange;
        }

        public ModifiableUnitStats(UnitStats unitDataUnitStats)
        {
            BaseMaxHP = unitDataUnitStats.MaxHP;
            BaseAttack =  unitDataUnitStats.Attack;
            BaseSpeed = unitDataUnitStats.Speed;
            BaseAttackCooldown = unitDataUnitStats.AttackCooldown;
            BaseAttackRange = unitDataUnitStats.AttackRange;
        }

        public void AddStats(UnitStats otherStats)
        {
            HPModifier += otherStats.MaxHP;
            AttackModifier += otherStats.Attack;
            SpeedModifier += otherStats.Speed;
            AttackCooldownModifier += otherStats.AttackCooldown;
            AttackRangeModifier += otherStats.AttackRange;
            CheckChange(otherStats);
        }
        
        public void SubtractStats(UnitStats otherStats)
        {
            HPModifier -= otherStats.MaxHP;
            AttackModifier -= otherStats.Attack;
            SpeedModifier -= otherStats.Speed;
            AttackCooldownModifier -= otherStats.AttackCooldown;
            AttackRangeModifier -= otherStats.AttackRange;
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
            };
        }
    }
}