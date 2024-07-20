namespace Spark.Gameplay.Entities.Common.Data
{
    public interface IDamagable
    {
        public float HealthMax { get; }
        public float Health { get; }

        public bool IsAlive { get; }

        public void Heal(float points);
        public void TakeDamage(float points);
        public void Die();
    }
}