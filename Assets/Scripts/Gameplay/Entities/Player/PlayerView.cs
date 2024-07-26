using Spark.Gameplay.Weapons;
using Spark.UI;
using UnityEngine;

namespace Spark.Gameplay.Entities.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] UIController _uiController;
        
        [SerializeField] Material _normal;
        [SerializeField] Material _invulner;
        MeshRenderer[] _playerMeshes;

        private void Start()
        {
            if (_uiController == null) _uiController = GetComponent<UIController>();

            _playerMeshes = gameObject.GetComponentsInChildren<MeshRenderer>();
        }

        public void UpdateHealtUI(float points)
        {
            _uiController.UpdatePlayerHealthUI(points);
        }

        public void UpdateWeaponUI(Weapon weapon)
        {
            _uiController.UpdatePlayerWeaponUI(weapon);
        }

        public void UpdateWeaponRangedAmmoUI(RangedWeapon rangedWeapon)
        {
            _uiController.UpdatePlayerRangedWeaponAmmoUI(rangedWeapon);
        }

        public void SetInvulner(bool enabled)
        {
            foreach (var renderer in _playerMeshes)
            {
                renderer.material = enabled ? _invulner : _normal;
            }
            gameObject.layer = SortingLayer.NameToID(enabled ? "Enemy" : "Player");
        }
    }
}