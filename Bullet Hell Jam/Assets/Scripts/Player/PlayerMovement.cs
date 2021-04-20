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
    
    private bool bossDefeated;

    private void OnEnable()
    {
        bossDefeated = false;

        BossController.OnBossDeath += DisableControls;
    }

    private void OnDisable()
    {
        BossController.OnBossDeath -= DisableControls;
    }

    private void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();

        input = GetComponent<InputController>();
    }

    private void FixedUpdate()
    {
        if (!bossDefeated)
        {
            HandleMovement();
            cam.CameraUpdatePosition(transform);
            cam.AutoScroll(transform);
        }
        else
            transform.position += Vector3.right * moveSpeed * Time.fixedDeltaTime;
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

    private void DisableControls()
    {
        bossDefeated = true;
    }
}
