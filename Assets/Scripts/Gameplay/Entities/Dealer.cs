using Spark.Gameplay.Entities.Common;
using Spark.Gameplay.Entities.Player;
using Spark.Gameplay.Items.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Gameplay.Entities
{
    public class Dealer : Actor, IInteractable
    {
        [SerializeField] GameObject _uiDealer;
        [SerializeField] PlayerController _playerController;

        public void Activate()
        {
            if (_uiDealer == null) return;

            if (!_uiDealer.activeInHierarchy)
            {
                _uiDealer.SetActive(true);
                _playerController.CanShoot = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                _uiDealer.SetActive(false);
                _playerController.CanShoot = true;
            }
        }
    }
}