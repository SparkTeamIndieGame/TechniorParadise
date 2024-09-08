using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Melee;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Ranged;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons
{
    public abstract class RefactoredWeapon<Data> : MonoBehaviour, IRefactoredWeapon where Data : RefactoredWeaponData
    {
        public Data current => data;
        [SerializeField] protected List<Data> _data = new();
        protected Data data;

        protected List<GameObject> _gameObjects = new();

        private float _actionReadyTime = .0f;

        protected bool isReady => Time.time >= _actionReadyTime;

        private void Start()
        {
            data = _data[0];

            InitializeWeaponGameObjectsAndDeactive();
        }

        public void Activate() 
        {
            if (!isReady) return;

            DoAction();
            // PlayParticleSystem(..., ...);
            StartCooldown();
        }

        public void Deactivate()
        {
            AbortAction();
        }

        public virtual void DisableAllGameObjectWeapons()
        {
            _gameObjects.ForEach(gameObject => gameObject.SetActive(false));
        }

        public virtual void ChangeWeapon(System.Enum type)
        {
            DisableAllGameObjectWeapons();
            data = _data.Find(data => data.type.Equals(type));
            _gameObjects.Find(weapon => weapon.name == data.prefab.name).SetActive(true);
        }

        protected abstract void DoAction();
        protected virtual void AbortAction() { }

        protected virtual void StartCooldown()
        {
            _actionReadyTime = Time.time + data.rate;
        }

        protected static void PlayParticleSystem(ParticleSystem particleSystem, Transform parent)
        {
            Destroy(
                Instantiate(
                    particleSystem,
                    parent.position,
                    Quaternion.identity,
                    parent
                ).gameObject, 1.0f
            );
        }

        private void InitializeWeaponGameObjectsAndDeactive()
        {
            GameObject child;
            for (int index = 0; index < transform.childCount; ++index)
            {
                child = transform.GetChild(index).gameObject;
                _gameObjects.Add(child);

                if (data.prefab.name != child.name) child.SetActive(false);
            }
        }
    }
}