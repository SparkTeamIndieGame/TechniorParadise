using Spark.Gameplay.Entities.Common;
using Spark.Gameplay.Entities.Common.Data;
using UnityEngine;

namespace Spark.Gameplay.Entities.Enemies
{
    public class Enemy : Pawn, IEnemy
    {
        [SerializeField] private float _healthMax;
        [SerializeField] private float _health;
        [SerializeField] private float _damage;

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
            if (Health <= 0) Die();
        }
        public void Die() => Destroy(gameObject);

        public void Attack(IDamagable damagable) => damagable.TakeDamage(_damage);
        public void Attack(IDamagable damagable, float damage) => damagable.TakeDamage(damage);
    }
}