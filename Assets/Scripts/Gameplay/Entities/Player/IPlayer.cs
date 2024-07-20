using Spark.Gameplay.Entities.Common.Behaviour;
using Spark.Gameplay.Entities.Common.Data;

namespace Spark.Gameplay.Entities.Player
{
    public interface IPlayer : IMovable, ITurnable, IDamagable, IAttackable
    {
        
    }
}