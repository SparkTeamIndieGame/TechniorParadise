using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector2 _input;
    private CharacterController _controller;
    private Vector3 _direction;

    //временно
    public float damage = 3.0f;
    public FindEnemy findEnemy;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _controller.Move(_direction * speed * Time.deltaTime);
        //временно
        if (Input.GetMouseButtonDown(0))
        {
            if (findEnemy._enemyTest != null)
            {
                findEnemy._enemyTest.TakeDamage(damage);
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }
}
