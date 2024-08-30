using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public LoaderScens Loader;
    private void OnTriggerEnter(Collider other)
    {
        Loader.NextScene();
    }
}
