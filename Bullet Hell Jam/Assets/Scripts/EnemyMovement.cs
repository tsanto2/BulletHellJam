using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyMovement : MonoBehaviour
{
    public MovementBehaviour movementBehaviour;
    [SerializeField] private float WakeUpDelay = 2f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDelay;

    private bool awake;
    private float moveStartTime;

    private EnemyController controller;

    private Camera cam;
    private CameraController camController;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    private void Start()
    {
        cam = Camera.main;
        camController = cam.GetComponent<CameraController>();
    }

    private void OnEnable()
    {
        awake = false;
    }

    private void FixedUpdate()
    {
        if (!awake)
            return;
        
        if (movementBehaviour != MovementBehaviour.MoveLeft &&
            movementBehaviour != MovementBehaviour.MoveRight)
        {
            camController.CameraUpdatePosition(transform);
        }
        if (Time.time >= moveStartTime)
            HandleMovement();
    }

    private void OnBecameVisible()
    {
        Invoke("WakeUp", WakeUpDelay);
    }

    private void HandleMovement()
    {
        switch (movementBehaviour)
        {
            case MovementBehaviour.MoveDown:
            {
                transform.position += Vector3.down * moveSpeed * Time.fixedDeltaTime;
                break;
            }

            case MovementBehaviour.MoveUp:
            {
                transform.position += Vector3.up * moveSpeed * Time.fixedDeltaTime;
                break;
            }

            case MovementBehaviour.MoveLeft:
            {
                transform.position += Vector3.left * moveSpeed * Time.fixedDeltaTime;

                if (cam.WorldToViewportPoint(transform.position).x < 0f)
                    controller.Die();
                break;
            }

            case MovementBehaviour.MoveRight:
            {
                transform.position += Vector3.right * moveSpeed * Time.fixedDeltaTime;

                if (cam.WorldToViewportPoint(transform.position).x > 1f)
                        controller.Die();
                break;
            }
        }
    }

    private void WakeUp()
    {
        awake = true;
        moveStartTime = Time.time + moveDelay;
        controller.WakeUp();
    }
}
