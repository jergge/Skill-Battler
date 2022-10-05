using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jergge.Extensions;
using UnityEngine.InputSystem;

namespace SkillSystem {


    public class PrayerOfMending : Skill, IActiveSkill
    {
        public float baseHealAmount = 50f;
        public int baseChargesCount = 5;
        protected int remainingCharges;
        public float baseJumpRange = 20f;
        public float projectileSpeed = 10f;

        public GameObject projectilePrefab;

        protected enum State {Projectile, Buff};
        protected State state;
        
        
        public void Cast(Transform spawnLoaction, TargetInfo targetInfo)
        {
            LivingEntity le;
            if (targetInfo.target.gameObject.TryGetComponent<LivingEntity>(out le))
            {
                var newThing = Instantiate(projectilePrefab);
                var pom = le.gameObject.AddComponent<PrayerOfMending>();

                pom.remainingCharges = baseChargesCount;
                pom.spellState = SpellState.InWorld;
            }
        }

        public override void OnStartInSpellbook()
        {
            base.OnStartInSpellbook();
        }

        public override void OnStartInWorld()
        {
            base.OnStartInWorld();
            LivingEntity le = GetComponent<LivingEntity>();
            le.OnTakeDamage += TriggerHeal;

        }

        public override void UpdateInSpellBook()
        {
            base.UpdateInSpellBook();
        }

        public override void UpdateInWorld()
        {
            base.UpdateInWorld();
        }

        void TriggerHeal(DamageInfo info)
        {
            if (info.amountDone >0 && !info.lethalHit)
            {
                var le = gameObject.GetComponent<LivingEntity>();
                le.TakeHeal(((int)baseHealAmount));
                le.OnTakeDamage -= TriggerHeal;
                if (remainingCharges > 0 )
                {
                    LivingEntity newTarget = GetInDistance<LivingEntity>(baseJumpRange).Random<LivingEntity>();

                    var pom = newTarget.gameObject.AddComponent<PrayerOfMending>();
                    pom.remainingCharges = remainingCharges -1;
                }
                Destroy(this);
            }
        } 
    }
}
