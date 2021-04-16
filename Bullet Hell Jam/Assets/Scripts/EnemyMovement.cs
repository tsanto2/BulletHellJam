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
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDelay;
    private float moveStartTime;

    private float scrollSpeed;
    private bool awake;

    private EnemyController controller;
    private Camera cam;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    private void Start()
    {
        scrollSpeed = FindObjectOfType<CameraController>().scrollSpeed;
        cam = Camera.main;
    }

    private void OnEnable()
    {
        awake = false;
    }

    private void FixedUpdate()
    {
        if (!awake)
            return;

        HandleAutoScroll();
        
        if (Time.time >= moveStartTime)
            HandleMovement();
    }

    private void OnBecameVisible()
    {
        Invoke("WakeUp", 2f);
    }

    private void HandleAutoScroll()
    {
        transform.position += Vector3.right * scrollSpeed * Time.fixedDeltaTime;
    }

    private void HandleMovement()
    {
        switch (movementBehaviour)
        {
            case MovementBehaviour.StrafeDown:
            {
                if (transform.position.y > -2f)
                    transform.position += Vector3.down * moveSpeed * Time.fixedDeltaTime;
                break;
            }

            case MovementBehaviour.StrafeUp:
            {
                if (transform.position.y < 2f)
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
