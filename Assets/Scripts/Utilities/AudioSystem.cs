using System;
using System.Collections.Generic;
using UnityEngine;
namespace Spark.Utilities
{
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
                AudioDictinory.Add(audioSources[i].gameObject.name, audioSources[i]);
            }
        }
    }

   

}


