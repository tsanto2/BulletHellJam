using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Min(0.1f)]
    public float scrollSpeed = 0.1f;

    private void FixedUpdate()
    {
        transform.position += Vector3.right * scrollSpeed * Time.fixedDeltaTime;
    }
}
