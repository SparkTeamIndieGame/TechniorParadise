using System;
using System.Collections.Generic;
using UnityEngine;
namespace Spark.Utilities
{
    [RequireComponent(typeof(AudioSource))]

    
    [Serializable]
    public class AudioSystem
    {
        [SerializeField] private AudioSource[] audioSources;
        public Dictionary<string, AudioSource> AudioDictinory = new Dictionary<string, AudioSource>();

        public void Instalize()
        {
            if (audioSources.Length == 0) return;

            for(int i =0; i < audioSources.Length; i++)
            {
                AudioDictinory.Add(audioSources[i].clip.name, audioSources[i]);
            }
        }
    }

   

}


