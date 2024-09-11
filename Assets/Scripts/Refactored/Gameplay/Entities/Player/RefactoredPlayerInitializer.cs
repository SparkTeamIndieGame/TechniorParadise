using Spark.Gameplay.Entities.RefactoredPlayer.UI;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
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
            var model = new Model();
            var view = Utils.LoadComponent<View>(gameObject);
            var ui = FindAnyObjectByType<RefactoredUIController>();

            new Controller(model, view, ui);
            Destroy(this);
        }
    }
}