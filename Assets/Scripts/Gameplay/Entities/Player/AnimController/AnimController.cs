using UnityEngine;
using System;
using Spark.Gameplay.Weapons;

namespace Spark.Gameplay.Entities.Player
{
    [Serializable]
    public class AnimController
    {
        private Animator _animator;
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


        public void SwitchAnimForTypeWeapon(Weapon weaponsType)
        {
            if((weaponsType is MeleeWeapon))
            {
                _animator.SetBool("MeleWeapon", true);
                _animator.SetBool("RangeWeapon", false);
            }

            else if((weaponsType is RangedWeapon))
            {
                _animator.SetBool("MeleWeapon", false);
                _animator.SetBool("RangeWeapon", true);
            }
        }

        public void AnimMove(Vector2 movement)
        {
            if (movement != Vector2.zero)
            {
                _animator.SetBool("Run", true);
            }

            else
            {
                _animator.SetBool("Run", false);
            }
        }
    }

}
