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
    }
}