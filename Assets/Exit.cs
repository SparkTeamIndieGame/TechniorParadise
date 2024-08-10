using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(TryGetComponent<CharacterController>(out CharacterController component))
        {
            SceneManager.LoadScene(0);
        }
    }
}
