using Spark.Gameplay.Entities.Common.Data;

namespace Spark.Gameplay.Entities.Common.Behaviour
{
    public interface IAttackable
    {
        public void Attack(IDamagable damagable);
        public void Attack(IDamagable damagable, float damage);
    }
}