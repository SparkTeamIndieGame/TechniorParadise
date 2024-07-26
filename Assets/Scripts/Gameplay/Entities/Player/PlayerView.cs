using Spark.Gameplay.Weapons;
using Spark.UI;
using UnityEngine;

namespace Spark.Gameplay.Entities.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] UIController _uiController;

        private void Start()
        {
            if (_uiController == null) _uiController = GetComponent<UIController>();
        }

        public void UpdateWeaponUI(Weapon weapon)
        {
            _uiController.UpdatePlayerWeaponUI(weapon);
        }

        public void UpdateWeaponRangedAmmoUI(RangedWeapon rangedWeapon)
        {
            _uiController.UpdatePlayerRangedWeaponAmmoUI(rangedWeapon);
        }
    }
}