using Spark.Gameplay.Entities.Player;
using Spark.Gameplay.Items.Interactable;
using UnityEngine;

namespace Spark.Gameplay.Items.Pickupable.Doors.Terminal
{
    public class FlashCardTerminalDoubleDoors : TerminalDoubleDoors
    {
        [SerializeField] PlayerController _player;

        public override void Activate()
        {
            if (_player.FlashCard.IsCollected)
            {
                _isActivated = true;
                _player.FlashCard.Reset();
            }
        }
    }
}