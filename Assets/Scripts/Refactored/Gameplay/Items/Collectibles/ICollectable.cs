using Spark.Refactored.Gameplay.Entities.Player.MVC;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Items
{
    public interface ICollectable
    {
        void Activate(View player);
        void Deactivate();
    }
}