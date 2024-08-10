using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Weapons;
using Spark.UI;
using UnityEngine;

namespace Spark.Gameplay.Entities.Enemies.Bosses.Crab
{
    public class CrabView : MonoBehaviour
    {
        [SerializeField] UIController _uiController;
        
        private void Start()
        {
            if (_uiController == null) _uiController = GetComponent<UIController>();
        }

        public void UpdateHealtUI(IDamagable damagable)
        {
            _uiController.UpdateBossHealthUI(damagable);
        }
    }
}