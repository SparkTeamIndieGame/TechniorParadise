using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;
using Spark.Gameplay.Entities.RefactoredPlayer.RefactoredSystems;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class RefactoredPlayerModel
    {
        public FlashAbility flashAbility { get; } = new();
        public InvulnerAbility invulnerAbility { get; } = new();

        public FlashDrive flashDrive { get; } = new();
        public float details { get; set; }
    }
}