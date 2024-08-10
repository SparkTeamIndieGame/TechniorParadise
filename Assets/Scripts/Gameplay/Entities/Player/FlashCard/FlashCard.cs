using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.Player.FlashCard
{
    [Serializable]
    public class FlashCard
    {
        [field: SerializeField, Range(0, 10)] public int MaxAmount { get; private set; }
        public int Count { get; private set; }

        public bool IsCollected => Count >= MaxAmount;

        public void Add() => ++Count;
        public void Reset() => Count = 0;
    }
}