using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float effect;
    [SerializeField] private bool reset;

    private float spriteWidth;
    private Vector3 startPosition;

    private void Awake()
    {
        spriteWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        startPosition = transform.localPosition;
    }
    
    private void FixedUpdate()
    {
        transform.localPosition += Vector3.left * effect * Time.fixedDeltaTime;

        if (!reset)
            return;

        if (transform.localPosition.x <= startPosition.x - spriteWidth)
            transform.localPosition = startPosition;
    }
}
