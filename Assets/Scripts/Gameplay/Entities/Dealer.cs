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

        public void Activate()
        {
            if (_uiDealer == null) return;

            if (!_uiDealer.activeInHierarchy) _uiDealer.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerController>() != null)
                _uiDealer.SetActive(false);
        }
    }
}