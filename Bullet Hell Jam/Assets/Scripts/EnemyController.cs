﻿using System.Collections;
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
    [SerializeField] private LayerMask playerBulletLayerMask;
    private Collider2D hit;

    private ObjectPool pool;
    private SpriteRenderer spriteRenderer;
    private float scrollSpeed;
    private bool awake;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        pool = FindObjectOfType<ObjectPool>();
        

        if (pool == null)
            Debug.LogError("Cannot find object pool");
    }

    private void OnEnable()
    {
        health = healthMax;
        awake = false;
    }

    private void FixedUpdate()
    {
        if (!awake)
            return;

        HandleShooting();
        
        CheckForPlayerBullets();
    }

    public void WakeUp()
    {
        awake = true;
    }

    private void CheckForPlayerBullets()
    {
        hit = Physics2D.OverlapCircle(transform.position, 1f, playerBulletLayerMask);

        if (hit)
        {
            pool.ReturnObject(hit.gameObject);
            TakeDamage(1);
        }
    }

    public void Die()
    {
        pool.ReturnObject(this.gameObject);
    }

    private void HandleShooting()
    {
        if (Time.time > shootCooldown)
            Shoot(weapon);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Ouch!");
        Health -= damage;

        if (health == 0)
            Die();
    }

    public void Shoot(BulletPattern weapon)
    {
        weapon.SpawnBullets(transform.position, this);
    }
}
