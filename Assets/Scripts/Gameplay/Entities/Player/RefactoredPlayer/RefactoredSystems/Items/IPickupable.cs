using Spark.Gameplay.Entities.RefactoredPlayer;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Items
{
    public interface IPickupable
    {
        void Activate(RefactoredPlayerView player);
    }
}