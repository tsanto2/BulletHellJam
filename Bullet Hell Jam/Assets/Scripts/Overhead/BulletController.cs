using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float startTime;
    [HideInInspector] public Vector3 moveVec;
    [HideInInspector] public float rotationSpeed;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float lifetime;
    public ObjectPool pool;
    private bool active;

    private Quaternion startRotation;

    public IBulletHitBehaviour BulletHitBehaviour { get; }

    private void Awake()
    {
        startRotation = transform.rotation;
    }

    protected virtual void OnEnable()
    {
        startTime = Time.time;
        transform.rotation = startRotation;
        active = true;
    }

    protected virtual void FixedUpdate()
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

    public void ReverseDirection()
    {
        moveVec *= -1f;
    }

    public virtual void Die(bool scorePoints)
    {
        
    }
}
