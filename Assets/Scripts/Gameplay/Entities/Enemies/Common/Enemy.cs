using Spark.Gameplay.Entities.Common;
using Spark.Gameplay.Entities.Common.Data;
using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.Enemies
{
    public abstract class Enemy : Pawn, IEnemy
    {
        public static Action<float> OnEnemyAttack;

        public event Action<IDamagable> OnHealthChanged;

        [SerializeField] protected Transform _target;

        [SerializeField] private float _healthMax;
        [SerializeField] private float _health;

        [SerializeField] protected float _damage;
        [SerializeField] protected float _attackRange;
        [SerializeField] protected LayerMask layerMask;

        [SerializeField] protected ParticleSystem _shootingParticleSystem;
        [SerializeField] protected ParticleSystem _impactParticleSystem;

        [Serializable] protected struct DropEnemyItem
        {
            [SerializeField] public GameObject Prefab;
            [SerializeField, Range(1, 100)] public int Chance;

            public int CalculateDropChance() => UnityEngine.Random.Range(0, 100);
        }

        [SerializeField] protected DropEnemyItem _dropDetailsPickup;
        [SerializeField] protected DropEnemyItem _dropAidKitPickup;

        [SerializeField, Min(.1f)] private float _attackDelay;
        private float _lastAttackTime;

        public float MaxHealth => _healthMax;
        public float Health
        {
            get => _health;
            private set => _health = value;
        }
        public bool IsAlive => Health > 0;

        public void Heal(float points) => Health = Mathf.Min(Health + points, MaxHealth);
        public void TakeDamage(float points)
        {
            Health -= points;
            OnHealthChanged?.Invoke(this);
            if (Health <= 0) Die();
        }
        public void Die()
        {
            TryToDrop(_dropDetailsPickup);
            TryToDrop(_dropAidKitPickup);

            Destroy(gameObject);
        }

        private void TryToDrop(DropEnemyItem dropInfo)
        {
            if (dropInfo.CalculateDropChance() <= dropInfo.Chance)
                Instantiate(dropInfo.Prefab, transform.position, Quaternion.identity, transform.parent.parent);
        }

        public void Attack()
        {
            if (_lastAttackTime + _attackDelay > Time.time) return;
            ParticlPlay(_shootingParticleSystem, transform);
            CalculateHit();
            _lastAttackTime = Time.time;
        }

        protected abstract void CalculateHit();

        public virtual void ParticlPlay(ParticleSystem particle, Transform positionSpawn)
        {
            var effect = Instantiate(particle, positionSpawn.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, 1);
        }
    }

}