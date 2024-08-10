using Spark.Gameplay.Entities.Player;

namespace Spark.Gameplay.Items.Pickupable
{
    interface IPickupable
    {
        void Activate(PlayerModel player);
    }
}