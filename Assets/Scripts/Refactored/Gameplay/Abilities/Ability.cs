using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Spark.Refactored.Gameplay.Abilities
{
    [Serializable]
    public abstract class Ability : ScriptableObject
    {
        #region Cooldown variables
        private float _abilityReadyTime;
        private float _abilityDeactivateTime;
        #endregion

        #region Ability attributes
        [field: SerializeField] public virtual Sprite sprite { get; protected set; }
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

        private void OnValidate()
        {
            _abilityReadyTime = .0f; 
            _abilityDeactivateTime = .0f;
        }

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

        public abstract void InstantiateForPlayer();

        protected abstract void DoAction();
        protected virtual void AbortAction() { }

        private void StartCooldown()
        {
            _abilityReadyTime = Time.time + cooldownDuration;
            _abilityDeactivateTime = Time.time + abilityDuration;
        }
    }
}