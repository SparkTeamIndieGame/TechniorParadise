using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Gameplay.Entities.Common
{
    [RequireComponent(typeof(Animator))]
    public class Actor : MonoBehaviour
    {
        [SerializeField] protected Animator Animator { get; set; }

        private void Start() 
        {
            if (Animator == null) Animator = GetComponent<Animator>();
        }
    }
}