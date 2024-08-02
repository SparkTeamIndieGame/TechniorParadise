using Spark.Gameplay.Entities.Common;
using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Entities.Player;
using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

namespace Spark.Gameplay.Entities.Enemies
{
    public class Enemy : Pawn, IEnemy
    {
        public static event IEnemy.EnemyAttack OnEnemyAttack;

        public event Action<IDamagable> OnHealthChanged;

        [SerializeField] protected Transform _target;

        [SerializeField] private float _healthMax;
        [SerializeField] private float _health;

        [SerializeField] protected float _damage;
        [SerializeField] protected float _attackRange;
        [SerializeField] protected float _distanceView;


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
            Destroy(gameObject);
        }
        public override void Update()
        {
            base.Update();

            if (alone)
            {
                float _distanceToTarget = Vector3.Distance(transform.position, _target.position);

                if (_distanceToTarget > _distanceView)
                {
                    ReturnToPatrol();

                }
                else if (_distanceToTarget <= _distanceView)
                {
                //    if (!Physics.Linecast(transform.position, _target.position) &&
                //_target.gameObject.layer == SortingLayer.NameToID("Player"))
                        MoveToTarget(_target.position);
                }

                else
                {
                    _animator.SetTrigger("Attack");
                }

            }

        }

        public void Attack()
        {
            if (_lastAttackTime + _attackDelay > Time.time) return;

            var hits = Physics.OverlapSphere(transform.position, _attackRange);
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent<PlayerController>(out var _))
                    OnEnemyAttack?.Invoke(_damage);
            }

            _lastAttackTime = Time.time;
        }
        public void Attack(IDamagable damagable) => damagable.TakeDamage(_damage);
        public void Attack(IDamagable damagable, float damage) => damagable.TakeDamage(damage);


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!alone) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _distanceView);
            Gizmos.DrawLine(transform.position, _target.position);
        }
#endif
    }

}