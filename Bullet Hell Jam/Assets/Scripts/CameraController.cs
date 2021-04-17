using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float minX = -8.5f;
    [SerializeField] private float maxX = 8.5f;
    [SerializeField] private float minY = -2.5f;
    [SerializeField] private float maxY = 3.5f;
    [SerializeField] private float scrollSpeed = 1.5f;

    private void FixedUpdate()
    {
        AutoScroll(transform);
    }

    public void CameraUpdatePosition(Transform transform)
    {

        float clampMinX = this.transform.position.x + minX;
        float clampMaxX = this.transform.position.x + maxX;
        float clampMinY = this.transform.position.y + minY;
        float clampMaxY = this.transform.position.y + maxY;

        float clampX = Mathf.Clamp(transform.position.x, clampMinX, clampMaxX);
        float clampY = Mathf.Clamp(transform.position.y, clampMinY, clampMaxY);
        transform.position = new Vector3(clampX, clampY);
    }

    public void AutoScroll(Transform transform)
    {
        transform.position += Vector3.right * scrollSpeed * Time.fixedDeltaTime;
    }
}
