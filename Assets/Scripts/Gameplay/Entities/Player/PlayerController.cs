using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
    typeof(CharacterController)
)]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;

    [SerializeField] private float _movementSpeed = 100.0f;
    [SerializeField] private float _rotationSpeed = 100.0f;

    private void Awake()
    {
        if (_controller == null) _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * moveInput * Time.deltaTime * _movementSpeed;
        _controller.Move(moveDirection);
    }

    private void HandleRotation()
    {
        float rotateInput = Input.GetAxis("Horizontal");
        Vector3 rotateDirection = Vector3.up * rotateInput * Time.deltaTime * _rotationSpeed;
        transform.Rotate(rotateDirection);
    }
}
