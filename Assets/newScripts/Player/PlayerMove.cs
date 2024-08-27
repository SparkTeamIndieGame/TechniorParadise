using Spark.Gameplay.Entities.Player;
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
    
    //вращение
    private float smoothTime = 0.05f;
    private float _currentVelocity;
    
    //анимация
    public AnimController animController;
    
    //стрельба
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float buletSpeed = 10.0f;
    public float fireRate = 0.5f;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        animController.AnimMove(_input);
        if (_input.sqrMagnitude != 0)
        {
            _controller.Move(_direction * speed * Time.deltaTime);
            var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg; 
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
           
        }

        if (findEnemy._closestEnemy != null)
        {
            transform.rotation =
                Quaternion.LookRotation(findEnemy._closestEnemy.transform.position - this.transform.position);

        }


    }

    // public void Shoot()
    // {
    //     GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
    //     bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * buletSpeed;
    // }
    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }
}
