using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.Common
{
    [Serializable]
    public abstract class Ability
    {
        #region Ability attributes
        [SerializeField] private string _name;
        public virtual string Name 
        {  
            get => _name;
            protected set => _name = value;
        }

        [SerializeField] private string _description;
        public virtual string Description
        {
            get => _description;
            protected set => _description = value;
        }

        [SerializeField] private float _cooldownDuration;
        protected virtual float CooldownDuration
        {
            get => _cooldownDuration;
            set => _cooldownDuration = value;
        }

        [SerializeField] private float _abilityDuration;
        protected virtual float AbilityDuration
        {
            get => _abilityDuration;
            set => _abilityDuration = value;
        }
        #endregion

        #region Public properties
        public float Cooldown => _cooldownTimeLeft;
        public bool IsReady => Time.time >= _nextReadyTime;
        public bool AbilityActive { get; private set; }
        #endregion

        #region Cooldown variables
        private float _cooldownTimeLeft;
        private float _nextReadyTime;
        private float _abilityTimeLeft;
        #endregion

        public virtual void Update()
        {
            AbilityActive = Time.time < _abilityTimeLeft;
            _cooldownTimeLeft = Mathf.Max(0, _nextReadyTime - Time.time);
        }

        public void Use()
        {
            if (!IsReady) return;

            DoAction();

            StartCooldown();
            AbilityActive = true;
        }

        public void Cancel()
        {
            AbortAction();

            _abilityTimeLeft = 0;
            AbilityActive = false;
        }

        protected abstract void DoAction();
        protected virtual void AbortAction() { }

        private void StartCooldown()
        {
            _nextReadyTime = Time.time + CooldownDuration;
            _abilityTimeLeft = Time.time + AbilityDuration;
            _cooldownTimeLeft = CooldownDuration;
        }
    }
}