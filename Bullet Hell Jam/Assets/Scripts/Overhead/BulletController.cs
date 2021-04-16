using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float startTime;
    public Vector3 moveVec;
    public float rotationSpeed;
    public float moveSpeed;
    [SerializeField] private float lifetime;
    public ObjectPool pool;
    private bool active;

    private SpriteRenderer spriteRenderer;
    private Material material;
    public BulletPattern weapon;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        material = new Material(spriteRenderer.material);
        spriteRenderer.material = material;
    }

    private void OnEnable()
    {
        startTime = Time.time;
        transform.rotation = Quaternion.identity;
        active = true;
    }

    private void FixedUpdate()
    {
        if (rotationSpeed != 0f)
            transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.fixedDeltaTime));

        transform.Translate(moveVec * moveSpeed * Time.fixedDeltaTime);

        if (Time.time > startTime + lifetime)
        {
            if (active)
            {
                active = false;
                pool.ReturnObject(this.gameObject);
            }
        }
    }

    public void ChangeColor(Color color)
    {
        material.SetColor("_Color", color);
    }
}
