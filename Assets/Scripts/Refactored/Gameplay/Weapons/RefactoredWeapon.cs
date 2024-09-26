using Spark.Gameplay.Entities.RefactoredPlayer.UI;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Melee;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Ranged;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons
{
    public abstract class RefactoredWeapon<Data> : MonoBehaviour, IRefactoredWeapon where Data : RefactoredWeaponData
    {
        RefactoredUIController _ui;

        public Data current => _currentData;

        protected Data _currentData;
        [SerializeField] protected List<Data> _database = new();
        [SerializeField] protected Image _icon;

        protected List<GameObject> _gameObjects = new();

        private float _actionReadyTime = .0f;

        protected bool isReady => Time.time >= _actionReadyTime;

        private void Start()
        {
            _ui = FindFirstObjectByType<RefactoredUIController>();

            _currentData = _database[0];

            InitializeWeaponGameObjectsAndDeactive();
            InitializeWeaponAbilities();
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

        public void ActivateAbility()
        {
            _currentData.ability?.Activate();
            _ui.UpdateWeaponAbilityIcon(_currentData.ability);
        }

        public virtual void DisableAllGameObjectWeapons()
        {
            _gameObjects.ForEach(gameObject => gameObject.SetActive(false));
        }

        public virtual void ChangeWeapon(System.Enum type)
        {
            if (!_currentData.ability.isReady) return;

            DisableAllGameObjectWeapons();
            _currentData = _database.Find(data => data.type.Equals(type));
            _gameObjects.Find(weapon => weapon.name == _currentData.prefab.name).SetActive(true);

            _ui.UpdateWeaponAbilityIcon(_currentData.ability);
        }

        protected abstract void DoAction();
        protected virtual void AbortAction() { }

        protected virtual void StartCooldown()
        {
            _actionReadyTime = Time.time + _currentData.rate;
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

                if (_currentData.prefab.name != child.name) child.SetActive(false);
            }
        }

        private void InitializeWeaponAbilities()
        {
            foreach (var database in _database)
            {
                database.ability?.InstantiateForPlayer();
            }
            _ui.UpdateWeaponAbilityIcon(_currentData.ability);
        }
    }
}