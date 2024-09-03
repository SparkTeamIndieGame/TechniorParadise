using UnityEngine;

namespace Spark.Utilities
{
    public static class Utils
    {
        public static void LoadComponent<T>(GameObject gameObject, out T instance) where T : Component
        {
            if (!gameObject.TryGetComponent(out instance))
            {
                instance = gameObject.AddComponent<T>();
            }
        }

        public static T LoadComponent<T>(GameObject gameObject) where T : Component
        {
            LoadComponent(gameObject, out T instance);
            return instance;
        }
    }
}