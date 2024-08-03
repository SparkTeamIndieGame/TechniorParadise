using UnityEngine;

namespace Spark.Gameplay.Weapons
{
    public class Weapon : ScriptableObject
    {
        [SerializeField] private string _name;
        public virtual string Name
        {
            get => _name;
            protected set => _name = value;
        }

        [SerializeField] private string _description;
        public virtual string Description
        {
            get => _description;
            protected set => _description = value;
        }

        [SerializeField, Min(1.0f)] private float _damage;
        public virtual float Damage
        {
            get => _damage;
            protected set => _damage = value;
        }

        [SerializeField, Min(0.5f)] private float _range;
        public virtual float Range
        {
            get => _range;
            protected set => _range = value;
        }

        [SerializeField] private Sprite _icon;
        public virtual Sprite Icon => _icon;

        [SerializeField] private GameObject _prefab;
        public virtual GameObject Prefab => _prefab;

        [SerializeField] private AnimationClip[] _clips;
        public virtual AnimationClip[] Clips => _clips;

        [SerializeField] private Animation _animation;
        public virtual Animation Animation => _animation;

        [SerializeField] private Animator _animator;
        public virtual Animator Animator => _animator;


        [SerializeField] protected ParticleSystem _shootingParticleSystem;
        [SerializeField] protected ParticleSystem _impactParticleSystem;

        public virtual void ParticlPlay(ParticleSystem particle, Transform positionSpawn)
        {
            var effect = Instantiate(particle, positionSpawn.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, 1);
        }
    }
}