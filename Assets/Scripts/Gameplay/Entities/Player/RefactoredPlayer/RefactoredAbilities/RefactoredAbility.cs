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
        [field: SerializeField] public virtual string name { get; protected set; }
        [field: SerializeField] public virtual string description { get; protected set; }

        [field: SerializeField, Min(0.0f)] public virtual float cooldownDuration { get; protected set; }
        [field: SerializeField, Min(0.0f)] public virtual float abilityDuration { get; protected set; }
        #endregion

        #region Public properties
        public float cooldown
        {
            get
            {
                if (!isActive && !isReady)
                {
                    Deactivate();
                }
                return Mathf.Max(0, _abilityReadyTime - Time.time);
            }
        }
        public bool isReady => Time.time >= _abilityReadyTime;
        public bool isActive => Time.time < _abilityDeactivateTime;
        #endregion

        public void Activate()
        {
            if (!isReady) return;

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
            _abilityReadyTime = Time.time + cooldownDuration;
            _abilityDeactivateTime = Time.time + abilityDuration;
        }
    }
}