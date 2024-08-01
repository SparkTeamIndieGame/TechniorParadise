using UnityEngine;

namespace Spark.Gameplay.Entities.Enemies
{
    public class RangedEnemy : Enemy
    {
        private void Awake()
        {
            canMove = false;
        }


    }
}