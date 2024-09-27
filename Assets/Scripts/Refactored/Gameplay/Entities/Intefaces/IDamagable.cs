using System;

namespace Spark.Refactored.Gameplay.Entities.Interfaces
{
    public interface IDamagable : IHealthable
    {
        public float healthMaximum { get; }
        public new float health { get; set; }

        public abstract void Die();
    }
}