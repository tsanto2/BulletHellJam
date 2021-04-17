using UnityEngine;
using System;

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

    private IBulletHitBehaviour bulletHitBehaviour;
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
    
    [SerializeField] private LayerMask enemyBulletLayerMask;
    private Collider2D hit;

    private void Awake()
    {
        input = GetComponent<InputController>();

        Health = healthMax;
        Energy = energyMax;

        bulletHitBehaviour = new DamagePlayerBehaviour();
    }

    private void OnEnable()
    {
        GameManager.OnTenSecondsPassed += RefreshEnergy;
    }

    private void OnDisable()
    {
        GameManager.OnTenSecondsPassed -= RefreshEnergy;
    }

    private void FixedUpdate()
    {        
        CheckForEnemyBullets();
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

    private void CheckForEnemyBullets()
    {
        hit = Physics2D.OverlapCircle(transform.position, 0.1f, enemyBulletLayerMask);

        if (hit)
        {
            //ObjectPool.Instance.ReturnObject(hit.gameObject);
            //TakeDamage(1);
            bulletHitBehaviour.Perform(hit.gameObject);
        }
    }

    public void Shoot(BulletPattern weapon)
    {
        if (weapon == null)
            return;

        weapon.SpawnBullets(transform.position, this);
    }

    public void ChangeWeapon(BulletPattern weapon)
    {
        if (weapon == null)
            return;
                
        this.weapon = weapon;
    }

    public void ChangeBulletHitBehaviour(IBulletHitBehaviour newBulletHitBehaviour)
    {
        bulletHitBehaviour = newBulletHitBehaviour;
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