using UnityEngine;
using UnityEngine.UI;

public class StartShowComix : MonoBehaviour
{
    [SerializeField] private ComixShow _comixShow;
    
    public void StartComix()
    {
        _comixShow.RunComix();
    }
}
