using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyMovement : MonoBehaviour
{
    private enum MovementBehaviour
    {
        Freeze,
        StrafeUp,
        StrafeDown,
        MoveForward
    }

    [SerializeField] private MovementBehaviour movementBehaviour;
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
        
        camController.CameraUpdatePosition(transform);

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
            case MovementBehaviour.StrafeDown:
            {
                transform.position += Vector3.down * moveSpeed * Time.fixedDeltaTime;
                break;
            }

            case MovementBehaviour.StrafeUp:
            {
                transform.position += Vector3.up * moveSpeed * Time.fixedDeltaTime;
                break;
            }

            case MovementBehaviour.MoveForward:
            {
                transform.position += Vector3.left * moveSpeed * Time.fixedDeltaTime;

                if (cam.WorldToScreenPoint(transform.position).x < 0f)
                    ObjectPool.Instance.ReturnObject(this.gameObject);
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
