﻿using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(InputController))]
public class PlayerController : MonoBehaviour, IDamageable, IFireable
{
    #region Events
    public static event Action<int> OnPlayerHealthChange;
    public static event Action<int> OnPlayerEnergyChange;
    public static event Action<bool> OnPlayerActivateSlowmo;
    #endregion   

    [Header("Stats")]
    [SerializeField, Min(1)] private int healthMax;
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
            OnPlayerHealthChange?.Invoke(health);
        }
    }

    [SerializeField, Min(1)] private int energyMax;
    [SerializeField]
    private int energy;
    public int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = Mathf.Clamp(value, 0, energyMax);
            OnPlayerEnergyChange?.Invoke(energy);
        }
    }

    [Header("Weapons")]
    [SerializeField] private BulletPattern weapon;
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

    private InputController input;
    public BulletCollisionCheck BulletCollisionCheck { get; private set; }
    
    [SerializeField] private LayerMask enemyBulletLayerMask;
    private Collider2D hit;

    public bool debugWeapons = false;

    private bool canShoot = false;

    public bool CanShoot
    {
        get
        {
            return canShoot;
        }
        set
        {
            canShoot = value;
        }
    }

    public IBulletHitBehaviour BulletHitBehaviour { get; private set; }

    private void Awake()
    {
        input = GetComponent<InputController>();

        Health = healthMax;
        Energy = energyMax;
    }

    private void OnEnable()
    {
        GameManager.OnTenSecondsPassed += RefreshEnergy;

        BulletHitBehaviour = new DamagePlayerBehaviour(this);

        if (!debugWeapons)
            weapon = null;
    }

    private void OnDisable()
    {
        GameManager.OnTenSecondsPassed -= RefreshEnergy;
    }

    private void FixedUpdate()
    {        
        HandleShooting();
    }

    private void RefreshEnergy()
    {
        Energy = energyMax;
    }

    private void HandleShooting()
    {
        if (input.keyInput.shootPress && Time.time > shootCooldown)
            Shoot(weapon);
    }

    public void Shoot(BulletPattern weapon)
    {
        if (weapon == null || (!canShoot && !debugWeapons))
            return;

        weapon.SpawnBullets(transform.position);
        ShootCooldown = weapon.shootDelay;
    }

    public void ChangeWeapon(BulletPattern weapon)
    {
        if (weapon == null)
            return;
                
        this.weapon = weapon;
    }

    public void ChangeBulletHitBehaviour(IBulletHitBehaviour newBulletHitBehaviour)
    {
        BulletHitBehaviour = newBulletHitBehaviour;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health == 0)
            Die();
    }

    public void Die(bool scorePoints = false)
    {
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        Debug.Log("Doug Dimmadome declares you dimma-dun-dead, son.");
    }

}