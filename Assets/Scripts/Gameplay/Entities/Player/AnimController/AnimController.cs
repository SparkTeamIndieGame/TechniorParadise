using UnityEngine;
using System;
using Spark.Gameplay.Weapons.MeleeWeapon;
using Spark.Gameplay.Weapons.RangedWeapon;
using Spark.Gameplay.Weapons;

namespace Spark.Gameplay.Entities.Player
{
    [Serializable]
    public class AnimController
    {
        [SerializeField] private Animator _animator;
        public Animator Animator { get => _animator; set => _animator = value; }

        /// <summary>
        /// Возвращает true, если выбранно оружие ближнего боя, иначе false.
        /// </summary>
        public bool GetTypeWeapon()
        {
            return _animator.GetBool("MeleWeapon");
        }

        public void AnimAttack(int currentWeapon)
        {
            _animator.SetTrigger("Attack");
            _animator.SetInteger("item", currentWeapon);
        }

        public void SwitchAnimForTypeWeapon(WeaponData weaponsType)
        {
            _animator.SetBool("MeleWeapon", weaponsType is MeleeWeaponData);
            _animator.SetBool("RangeWeapon", weaponsType is RangedWeaponData);
        }

        public void AnimMove(Vector2 movement)
        {
            _animator.SetBool("Run", movement != Vector2.zero);
        }
    }
}
