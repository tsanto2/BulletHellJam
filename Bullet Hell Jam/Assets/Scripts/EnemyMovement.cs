using UnityEngine;
using System;

[RequireComponent(typeof(EnemyController))]
public class EnemyMovement : MonoBehaviour
{
    public static event Action<int> OnEnemyDespawnOffscreen;

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
        
        camController.AutoScroll(transform);
        
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
        float adjustedSpeed = moveSpeed * Time.fixedDeltaTime;

        switch (movementBehaviour)
        {
            case MovementBehaviour.MoveDown:
            {
                transform.position += Vector3.down * adjustedSpeed;
                break;
            }

            case MovementBehaviour.MoveUp:
            {
                transform.position += Vector3.up * adjustedSpeed;
                break;
            }

            case MovementBehaviour.MoveLeft:
            {
                transform.position += Vector3.left * adjustedSpeed;

                if (cam.WorldToViewportPoint(transform.position).x < 0f)
                {
                    OnEnemyDespawnOffscreen?.Invoke(1);
                    controller.Die(false);
                }
                break;
            }

            case MovementBehaviour.MoveRight:
            {
                transform.position += Vector3.right * adjustedSpeed;

                if (cam.WorldToViewportPoint(transform.position).x > 1f)
                {
                    OnEnemyDespawnOffscreen?.Invoke(1);
                    controller.Die(false);
                }
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
