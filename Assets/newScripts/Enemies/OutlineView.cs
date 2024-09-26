using System;
using UnityEngine;

public class OutlineView : MonoBehaviour
{
    private Outline _outline;

    private void Awake()
    {
        _outline = transform.GetComponentInParent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
    }

    private void Start()
    {
        GreenOutline();
    }

    public void GreenOutline()
    {
        _outline.OutlineWidth = 3.0f;
        _outline.OutlineColor = Color.green;
    }

    public void RedOutline()
    {
        _outline.OutlineWidth = 8.0f;
        _outline.OutlineColor = Color.red;
    }

    public void OrangeOutline()
    {
        _outline.OutlineWidth = 5.5f;
        _outline.OutlineColor = new Color32(255, 90, 0, 255);
    }

    private void OnTriggerEnter(Collider other)
    {
        OrangeOutline();
    }

    private void OnTriggerExit(Collider other)
    {
        GreenOutline();
    }
}
