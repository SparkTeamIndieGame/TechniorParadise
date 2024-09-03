using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer.RefactoredSystems
{
    public class FlashDrive
    {
        [field: SerializeField, Range(0, 10)] public int maximum { get; private set; } = 10;
        [field: SerializeField, Range(0, 10)] public int count { get; private set; }

        public bool isCollected => count >= maximum;

        public void Add() => ++count;
        public void Reset() => count = 0;
    }
}