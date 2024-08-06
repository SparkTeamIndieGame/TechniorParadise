using UnityEngine;

namespace Spark.Gameplay.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {       
        public abstract WeaponData Data { get; set; }
    }
}