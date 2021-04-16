using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float crawlSpeed;

    private CameraController cam;
    private InputController input;

    private void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();

        input = GetComponent<InputController>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        cam.CameraUpdatePosition(transform);
    }

    private void HandleMovement()
    {
        if (input.keyInput.crawlPress)
            Move(crawlSpeed);
        else
            Move(moveSpeed);
    }

    private void Move(float speed)
    {
        transform.position += input.keyInput.moveVec.normalized * speed * Time.fixedDeltaTime;
    }
}
