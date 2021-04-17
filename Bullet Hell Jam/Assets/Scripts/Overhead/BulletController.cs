using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float startTime;
    public Vector3 moveVec;
    public float rotationSpeed;
    public float moveSpeed;
    public float lifetime;
    public ObjectPool pool;
    private bool active;

    public BulletPattern weapon;
    private Quaternion startRotation;

    private void Awake()
    {
        startRotation = transform.rotation;
    }

    private void OnEnable()
    {
        startTime = Time.time;
        transform.rotation = startRotation;
        active = true;
    }

    private void FixedUpdate()
    {
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
}
