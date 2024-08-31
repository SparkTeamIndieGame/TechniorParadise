using Spark.Gameplay.Entities.RefactoredPlayer.Abilities;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class RefactoredPlayerModel
    {
        public FlashAbility flashAbility { get; } = new();
        public InvulnerAbility invulnerAbility { get; } = new();
    }
}