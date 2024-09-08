using Spark.Gameplay.Entities.RefactoredPlayer.UI;
using Spark.Utilities;
using System;
using UnityEngine;

namespace Spark.Gameplay.Entities.RefactoredPlayer
{
    public class RefactoredPlayerInitializer : MonoBehaviour
    {
        private void Start()
        {
            InitializeMVC();
        }

        private void InitializeMVC()
        {
            var model = new RefactoredPlayerModel();
            var view = Utils.LoadComponent<RefactoredPlayerView>(gameObject);
            var ui = FindAnyObjectByType<RefactoredUIController>();

            new RefactoredPlayerController(model, view, ui);
            Destroy(this);
        }
    }
}