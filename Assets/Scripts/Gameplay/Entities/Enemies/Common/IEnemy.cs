using System;
using Spark.Gameplay.Entities.Common.Behaviour;
using Spark.Gameplay.Entities.Common.Data;

namespace Spark.Gameplay.Entities.Enemies
{
    public interface IEnemy : IDamagable
    {
        public static Action<float> OnEnemyAttack;
    }
}