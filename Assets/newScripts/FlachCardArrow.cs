using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlachCardArrow : DirectionArrow
{
    public List<GameObject> FlachCards;
    private bool isReapit = false;
    public override void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.size = new Vector2(_spriteRenderer.size.x, 0.0f);
    }

    public override void Update()
    {
        CheckFlach();
        if (Input.GetMouseButton(0) && FlachCards.Count == 1)
        {
            var directionVector = (FlachCards[0].transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionVector, Vector3.up);
            transform.rotation = Quaternion.Euler(90.0f,lookRotation.eulerAngles.y, 0.0f);
            ShowArrow(FlachCards[0], isReapit);
        }
    }

    private void CheckFlach()
    {
        for (int i = 0; i < FlachCards.Count; i++)
        {
            if (FlachCards[i] == null)
            {
                FlachCards.RemoveAt(i);
            }
        }
    }
}
