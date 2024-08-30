using System.Collections;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{ 
    [SerializeField] private Transform _target;
    [SerializeField] private float _timeDelay = 5.0f;
    
    private SpriteRenderer spriteRenderer;
    private bool courutine = true;
    
    private void Start()  //запустить корутину
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, 0.0f);
        StartCoroutine(ArrowShow());
    }

    private void Update()
    {
        //направление
        var directionVector = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionVector, Vector3.up);
        transform.rotation = Quaternion.Euler(90.0f,lookRotation.eulerAngles.y, 0.0f);
        
        //удалить
        if (Input.GetKeyDown(KeyCode.Space))
        {
            courutine = false; // остановить
        }
    }
    //удалить или изменить
    private void ShowArrow()
    {
        StopAllCoroutines();
        StartCoroutine(ArrowShow());
    }

    private IEnumerator ArrowShow()
    {
        while (courutine)
        {
            for (float t = 0.0f; t <= 11.5f; t += Time.deltaTime * 6.0f)
            {
                spriteRenderer.size = new Vector2(spriteRenderer.size.x, t);
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f,t /10.0f );
                yield return null;
            }

            for (float t = 1.0f; t >= 0.0f; t -= Time.deltaTime * 2.0f)
            {
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f,t );
                yield return null;
            }
            yield return new WaitForSeconds(_timeDelay); // указать время задержки
        }
    }
}