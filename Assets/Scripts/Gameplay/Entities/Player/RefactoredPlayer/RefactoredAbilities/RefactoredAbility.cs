using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer.Abilities
{
    [Serializable]
    public abstract class RefactoredAbility
    {
        #region Cooldown variables
        private float _abilityReadyTime;
        private float _abilityDeactivateTime;
        #endregion

        #region Ability attributes
        [field: SerializeField] public virtual string Name { get; protected set; }
        [field: SerializeField] public virtual string Description { get; protected set; }

        [field: SerializeField, Min(0.0f)] public virtual float CooldownDuration { get; protected set; }
        [field: SerializeField, Min(0.0f)] public virtual float AbilityDuration { get; protected set; }
        #endregion

        #region Public properties
        public float Cooldown
        {
            get
            {
                if (!AbilityActive && !IsReady)
                {
                    Deactivate();
                }
                return Mathf.Max(0, _abilityReadyTime - Time.time);
            }
        }
        public bool IsReady => Time.time >= _abilityReadyTime;
        public bool AbilityActive => Time.time < _abilityDeactivateTime;
        #endregion

        public void Activate()
        {
            if (!IsReady) return;

            DoAction();
            StartCooldown();
        }

        public void Deactivate()
        {
            AbortAction();
            _abilityDeactivateTime = 0;
        }

        protected abstract void DoAction();
        protected virtual void AbortAction() { }

        private void StartCooldown()
        {
            _abilityReadyTime = Time.time + CooldownDuration;
            _abilityDeactivateTime = Time.time + AbilityDuration;
        }
    }
}