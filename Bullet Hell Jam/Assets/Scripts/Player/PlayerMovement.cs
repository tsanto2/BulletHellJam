using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float crawlSpeed;

    private Camera cam;
    private CamSettings camSettings;
    private InputController input;

    private void Start()
    {
        cam = Camera.main;
        camSettings = FindObjectOfType<CameraController>().camSettings;

        input = GetComponent<InputController>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ClampPositionToCamera();
    }

    private void ClampPositionToCamera()
    {
        Vector3 camPosition = cam.transform.position;

        float clampX = Mathf.Clamp(transform.position.x, camPosition.x + camSettings.minX, camPosition.x + camSettings.maxX);
        float clampY = Mathf.Clamp(transform.position.y, camPosition.y + camSettings.minY, camPosition.y + camSettings.maxY);
        transform.position = new Vector3(clampX, clampY);
    }

    private void HandleMovement()
    {
        transform.position += Vector3.right * camSettings.scrollSpeed * Time.fixedDeltaTime;

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
