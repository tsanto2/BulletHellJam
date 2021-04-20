using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Parallax")]
    [SerializeField] private float scrollSpeed = 1.5f;
    [SerializeField] private ParallaxItem[] parallaxItems;

    [Header("Camera Bounds")]
    [SerializeField] private float minX = -8.5f;
    [SerializeField] private float maxX = 8.5f;
    [SerializeField] private float minY = -2.5f;
    [SerializeField] private float maxY = 3.5f;

    private bool bossDefeated;

    private void OnEnable()
    {
        bossDefeated = false;

        BossController.OnBossDeath += DisableAutoScroll;
    }

    private void OnDisable()
    {
        BossController.OnBossDeath -= DisableAutoScroll;
    }

    private void FixedUpdate()
    {
        if (!bossDefeated)
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

    private void DisableAutoScroll()
    {
        bossDefeated = true;
    }

    [Serializable]
    private class ParallaxItem
    {
        public GameObject parallaxObject;
        public float speed;
        public int tiles;
    }
}
