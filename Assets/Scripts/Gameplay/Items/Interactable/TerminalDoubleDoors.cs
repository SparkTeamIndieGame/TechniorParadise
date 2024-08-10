using Spark.Gameplay.Entities.Player;
using Spark.Gameplay.Items.Interactable;
using UnityEngine;

namespace Spark.Gameplay.Items.Pickupable.Doors.Terminal
{
    public class TerminalDoubleDoors : AutoDoubleDoors, IInteractable
    {
        void FixedUpdate()
        {
            if (_isUsed) return;
            if (_isActivated) OpenDoubleDoors();
        }

        public override void Activate(PlayerModel player) { } // cleared for IPickupable
        public virtual void Activate() => _isActivated = true;
    }
}