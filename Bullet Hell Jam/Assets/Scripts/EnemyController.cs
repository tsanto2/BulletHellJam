using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
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

    private ObjectPool pool;

    private void Start()
    {
        pool = FindObjectOfType<ObjectPool>();
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
}
