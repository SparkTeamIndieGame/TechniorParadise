namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons
{
    public interface IRefactoredWeapon
    {
        public void Activate();
        public void Deactivate();
        public void ActivateAbility();

        public void ChangeWeapon(System.Enum weapon);
        public void DisableAllGameObjectWeapons();
    }
}