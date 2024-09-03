using System.Collections;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{ 
    public static GameObject TargetNew;
    [SerializeField] protected float _timeDelay = 30.0f;
    [SerializeField] protected float _speedCof = 20.0f;
    [SerializeField] protected GameObject _player;
    
    protected SpriteRenderer _spriteRenderer;
    private bool isRepit = true;
    private float _needDis;
    private float _currentDis;
    
    public virtual void Start()  //запустить корутину
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.size = new Vector2(_spriteRenderer.size.x, 0.0f);
        StartCoroutine(ArrowShow(TargetNew, isRepit));
    }

    public virtual void Update()
    {
        //направление
        var directionVector = (TargetNew.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionVector, Vector3.up);
        transform.rotation = Quaternion.Euler(90.0f,lookRotation.eulerAngles.y, 0.0f);
        
    }
    //удалить или изменить
    public virtual void ShowArrow(GameObject target, bool isRepitloop)
    {
        StopAllCoroutines();
        StartCoroutine(ArrowShow(target, isRepitloop));
    }

    public virtual IEnumerator ArrowShow(GameObject target, bool isRepit)
    {
        // while (true)
        // {
        //     transform.position = _player.transform.position;
        //     for (float t = 0.0f; t <= Mathf.Infinity; t += Time.deltaTime * _speedCof)
        //     {
        //         _spriteRenderer.size = new Vector2(_spriteRenderer.size.x, t);
        //         _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f,t /10.0f );
        //         _needDis = Vector3.Distance(transform.position, target.transform.position);
        //         _currentDis = _spriteRenderer.size.y * this.transform.localScale.x;
        //         if (_currentDis >= _needDis)
        //         {
        //             break;
        //         }
        //         yield return null;
        //     }
        //
        //     for (float t = 1.0f; t >= 0.0f; t -= Time.deltaTime * 2.0f)
        //     {
        //         _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f,t );
        //         yield return null;
        //     }
        //     yield return new WaitForSeconds(_timeDelay);
        // }
        do
        {
            transform.position = _player.transform.position;
            for (float t = 0.0f; t <= 15.0; t += Time.deltaTime * _speedCof)
            {
                _spriteRenderer.size = new Vector2(_spriteRenderer.size.x, t);
                _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f,t /10.0f );
                _needDis = Vector3.Distance(transform.position, target.transform.position);
                _currentDis = _spriteRenderer.size.y * this.transform.localScale.x;
                if (_currentDis >= _needDis)
                {
                    break;
                }
                yield return null;
            }

            for (float t = 1.0f; t >= 0.0f; t -= Time.deltaTime * 2.0f)
            {
                _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f,t );
                yield return null;
            }
            yield return new WaitForSeconds(_timeDelay);
        } while (isRepit);
    }
}