using Spark.Utilities;
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

            new RefactoredPlayerController(model, view);
            Destroy(this);
        }
    }
}