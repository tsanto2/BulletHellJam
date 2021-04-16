using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable, IFireable
{
    [SerializeField] private int healthMax;
    private int health;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = Mathf.Clamp(value, 0, healthMax);
        }
    }

    private float shootCooldown;
    public float ShootCooldown
    {
        get
        {
            return shootCooldown;
        }
        set
        {
            shootCooldown = Time.time + value;
        }
    }

    [SerializeField] private BulletPattern weapon;

    private ObjectPool pool;

    private void Start()
    {
        pool = FindObjectOfType<ObjectPool>();

        if (pool == null)
            Debug.LogError("Cannot find object pool");
    }

    private void OnEnable()
    {
        health = healthMax;
    }

    public void Die()
    {
        pool.ReturnObject(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (health == 0)
            Die();
    }

    public void Shoot(BulletPattern weapon)
    {
        throw new System.NotImplementedException();
    }
}
