using Spark.Gameplay.Entities.RefactoredPlayer.UI;
using Spark.Refactored.Gameplay.Abilities;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using Spark.Refactored.Gameplay.System.Checkpoint;
using Spark.Utilities;
using System;
using UnityEngine;

namespace Spark.Refactored.Gameplay.Entities.Player
{
    public class PlayerInitializer : MonoBehaviour
    {
        [SerializeField] private MedKitAbility _medkitAbility;

        private void Start()
        {
            InitializeMVC();
        }

        private void InitializeMVC()
        {
            var model = new Model(_medkitAbility);
            var view = Utils.LoadComponent<View>(gameObject);
            var ui = FindFirstObjectByType<RefactoredUIController>();
            var checkpointManager = FindFirstObjectByType<CheckpointManager>();

            new Controller(model, view, ui, checkpointManager);
            Destroy(this);
        }
    }
}