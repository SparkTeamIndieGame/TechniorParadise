using Spark.Refactored.Gameplay.Abilities;
using UnityEngine;

namespace Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons
{
    public abstract class RefactoredWeaponData : ScriptableObject
    {
        public abstract System.Enum type { get; }

        // [field: SerializeField] public new string name { get; private set; }
        [field: SerializeField] public string description { get; private set; }

        [field: SerializeField, Min(1.0f)] public float damage {  get; private set; }

        // [field: SerializeField] public Sprite icon { get; private set; }
        [field: SerializeField] public GameObject prefab { get; private set; }

        // [field: SerializeField] public ParticleSystem impactParticleSystem { get; private set; }
        // [field: SerializeField] public ParticleSystem hitEntityParticleSystem { get; private set; }
        // [field: SerializeField] public AudioClip audioClip { get; private set; }
        // [field: SerializeField] public int price { get; private set; }

        [field: SerializeField] public float rate { get; private set; }

        [field: SerializeField] public Ability ability { get; private set; }
    }
}