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

        public float HealthMax => _healthMax;
        public float Health
        {
            get => _health;
            private set
            {
                float points = value;

                if (points > 0) _health = Mathf.Min(Health + points, HealthMax);
                else if (points < 0)
                {
                    _health -= points;
                    if (_health <= 0) Die();
                }
            }
        }
        public bool IsAlive => Health > 0;

        public void Heal(float points) => Health += points;
        public void TakeDamage(float points) => Health -= points;
        public void Die() => Destroy(gameObject);

        public void Attack(IDamagable damagable) => damagable.TakeDamage(_damage);
        public void Attack(IDamagable damagable, float damage) => damagable.TakeDamage(damage);
    }
}