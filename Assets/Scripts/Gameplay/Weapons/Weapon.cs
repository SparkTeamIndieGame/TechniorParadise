using UnityEngine;

namespace Spark.Gameplay.Weapons
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class Weapon : MonoBehaviour
    {       
        public abstract WeaponData Data { get; set; }
        protected AudioSource audioSource;

        private void Awake()
        {
            if (!GetComponent<AudioSource>()) gameObject.AddComponent<AudioSource>();

            audioSource = GetComponent<AudioSource>();
        }
        protected void PlaySound(AudioClip clip, AudioSource source)
        {
            source.clip = clip;
            source.Play();
        }
    }
}