using System;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public interface IDamagable : IHealthable
    {
        public float healthMaximum { get; }
        public new float health { get; set; }

        public abstract void Die();
    }
}