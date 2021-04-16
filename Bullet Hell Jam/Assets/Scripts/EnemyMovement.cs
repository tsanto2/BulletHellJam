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
        StrafeDown
    }

    [SerializeField] private MovementBehaviour movementBehaviour;
    [SerializeField] private float moveSpeed;

    private float scrollSpeed;
    private bool awake;

    private EnemyController controller;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    private void Start()
    {
        scrollSpeed = FindObjectOfType<CameraController>().scrollSpeed;
    }

    private void FixedUpdate()
    {
        if (!awake)
            return;

        HandleAutoScroll();
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
        }
    }

    private void WakeUp()
    {
        awake = true;
        controller.WakeUp();
    }


}
