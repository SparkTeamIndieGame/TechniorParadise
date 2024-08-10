using UnityEngine;

namespace Spark.Gameplay.Weapons
{
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

        [field: SerializeField, Min(1.0f)] public float AttackDamage {  get; private set; }
        [field: SerializeField, Min(0.5f)] public float AttackRange { get; private set; }

        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }

        [field: SerializeField] public ParticleSystem ImpactParticleSystem { get; private set; }
        [field: SerializeField] public ParticleSystem HitEntityParticleSystem { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }

        public virtual void PlayParticleSystem(ParticleSystem particleSystem, Transform parent)
        {
            Destroy(
                Instantiate(
                    particleSystem, 
                    parent.position, 
                    Quaternion.identity, 
                    parent
                ).gameObject, 1.0f
            );
        }
    }
}