using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Min(0.1f)] public float scrollSpeed = 0.1f;

    public CamSettings camSettings = new CamSettings
    {
        minX = -8.5f,
        maxX = 8.5f,
        minY = -2.5f,
        maxY = 3.5f,
        scrollSpeed = 1.5f
    };

    private void FixedUpdate()
    {
        transform.position += Vector3.right * scrollSpeed * Time.fixedDeltaTime;
    }
}
