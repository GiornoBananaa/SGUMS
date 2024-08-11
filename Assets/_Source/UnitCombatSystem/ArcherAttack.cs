using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

namespace UnitCombatSystem
{
    public class ArcherAttack: AUnitAttack
    {
        public override UnitType UnitType => UnitType.Archer;
        
        protected override IEnumerable<IAttackModifier> AttackModifiers => new IAttackModifier[]
        {
            new AttackByHeightModifier(-20f, 20f, 0.7f, 1.5f),
            new AttackByRotationModifier(new AttackByRotationModifier.RotationDiapason[]
            {
                new (120, 240, 0f, new [] { UnitType.Shield })
            }),
            new AttackByChanceModifier(new AttackByChanceModifier.AttackChanceByType[]
            {
                new (0.8f, new [] { UnitType.Cavalry }),
            }),
        };
        
        protected override void StartAnimation(Unit unit, Unit enemy)
        {
            unit.Animator.SetTrigger(ATTACK_ANIMATOR_TRIGGER);
            var mainEffect = unit.AttackEffect.main;
            float speed = mainEffect.startSpeed.constant;
            float gravity = mainEffect.gravityModifier.constant * Physics.gravity.magnitude;
            
            float angle = GetProjectileLaunchAngle(unit, enemy,speed,gravity);
            
            unit.AttackEffect.transform.LookAt(enemy.transform);
            var rotation = unit.AttackEffect.transform.rotation.eulerAngles;
            unit.AttackEffect.transform.rotation = Quaternion.Euler(angle, rotation.y, rotation.z);
            unit.AttackEffect.Play();
        }
        
        protected override void StopAnimation(Unit unit, Unit enemy)
        {
            var effect = unit.AttackEffect;
            if(effect.particleCount == 0) return;
            
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[effect.particleCount];
            effect.GetParticles(particles);
            particles[0].remainingLifetime = 0;
            
            effect.SetParticles(particles, particles.Length);
        }
        
        protected override float GetAttackDelay(Unit unit, Unit enemy)
        {
            var mainEffect = unit.AttackEffect.main;
            float speed = mainEffect.startSpeed.constant;
            float gravity = mainEffect.gravityModifier.constant * Physics.gravity.magnitude;
            
            float angle = GetProjectileLaunchAngle(unit, enemy, speed, gravity);
            float arrowFlightTime = GetProjectileFlightTime(unit, enemy, speed, gravity, angle);
            
            Debug.Log("arrowFlightTime: " + arrowFlightTime);
            return arrowFlightTime;
        }
        
        private float GetProjectileFlightTime(Unit unit, Unit enemy, float speed, float gravity, float angle)
        {
            Vector3 start = unit.AttackEffect.transform.position;
            Vector3 end = enemy.transform.position;
            
            float arrowFlightTime = Mathf.Abs((speed * Mathf.Sin(angle) + Mathf.Sqrt(Mathf.Abs(
                speed * Mathf.Sin(angle) * (speed *  Mathf.Sin(angle)) + 2 * gravity * (start.y - end.y)
            ))) / gravity);
            float arrowFlightTime2 = Mathf.Abs((speed * Mathf.Sin(angle) - Mathf.Sqrt(Mathf.Abs(
                speed * Mathf.Sin(angle) * (speed *  Mathf.Sin(angle)) + 2 * gravity * (start.y - end.y)
            ))) / gravity);
            
            return arrowFlightTime > arrowFlightTime2 ? arrowFlightTime : arrowFlightTime2 ;
        }
        
        private float GetProjectileLaunchAngle(Unit unit, Unit enemy, float speed, float gravity)
        {
            Vector3 start = unit.AttackEffect.transform.position;
            Vector3 end = enemy.transform.position;
            
            var dir = end-start;
            var vSqr = speed*speed;
            float x = dir.sqrMagnitude;
            float y = dir.y;
            
            float bottom = gravity * Mathf.Sqrt(x);
            float uRoot = vSqr*vSqr - gravity * (gravity * x + (2.0f * y * vSqr));
            uRoot = Mathf.Sqrt(Mathf.Abs(uRoot));
            
            if (uRoot < 0.0f) 
            {
                Debug.Log("!target out of range!");
            }
            
            float angle     = -(Mathf.Atan2 (vSqr - uRoot, bottom) * Mathf.Rad2Deg);
            float highAngle = -(Mathf.Atan2 (vSqr + uRoot, bottom) * Mathf.Rad2Deg);
            
            return angle;
        }
    }
}