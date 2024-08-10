using Spark.Gameplay.Entities.Common;
using Spark.Gameplay.Items.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Gameplay.Entities
{
    public class Dealer : Actor, IInteractable
    {
        public void Activate()
        {
            Debug.Log("Hello! I'm Dealer!");
        }
    }
}