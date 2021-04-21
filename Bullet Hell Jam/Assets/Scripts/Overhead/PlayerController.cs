using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(InputController), typeof(BulletSpawner))]
public class PlayerController : MonoBehaviour, IDamageable, IFireable
{
    #region Events
    public static event Action<int> OnPlayerHealthChange;
    public static event Action<int> OnPlayerEnergyChange;
    public static event Action<bool> OnPlayerActivateSlowmo;
    public static event Action OnPlayerTakeDamage;
    public static event Action OnPlayerDied;
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

    public BulletSpawner Spawner { get; private set; }

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
    public bool debugNeverDie = false;

    [SerializeField]
    private BulletPattern peaShooter;
    [SerializeField]
    private SpriteRenderer bubbleSprite;

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
        Spawner = GetComponent<BulletSpawner>();
    }

    private void OnEnable()
    {
        GameManager.OnTenSecondsPassed += RefreshEnergy;
        AbsorbBullets.OnAbsorbBulletsCardActivated += EnlargeHitbox;
        AbilityManager.OnAbsorbBulletsCardDeactivated += ShrinkHitbox;
        BlockBullets.OnBlockBulletsCardActivated += EnlargeHitbox;
        AbilityManager.OnBlockBulletsCardDeactivated += ShrinkHitbox;
        AbilityManager.OnPlayerWeaponCardDeactivated += SetPeaShooter;

        BulletHitBehaviour = new DamagePlayerBehaviour(this);

        Health = healthMax;
        Energy = energyMax;

        /*if (!debugWeapons)
            Spawner.Pattern = null;*/
        SetPeaShooter();
    }

    private void OnDisable()
    {
        GameManager.OnTenSecondsPassed -= RefreshEnergy;
        AbsorbBullets.OnAbsorbBulletsCardActivated -= EnlargeHitbox;
        AbilityManager.OnAbsorbBulletsCardDeactivated -= ShrinkHitbox;
        BlockBullets.OnBlockBulletsCardActivated -= EnlargeHitbox;
        AbilityManager.OnBlockBulletsCardDeactivated -= ShrinkHitbox;
        AbilityManager.OnPlayerWeaponCardDeactivated -= SetPeaShooter;
    }

    private void FixedUpdate()
    {     
        HandleShooting();
    }

    private void RefreshEnergy()
    {
        Energy = energyMax;
    }

    private void EnlargeHitbox(float duration)
    {
        BulletCollisionCheck bcheck = GetComponent<BulletCollisionCheck>();
        bcheck.ChangeToLargeRadius();
        bubbleSprite.enabled = true;
    }

    private void ShrinkHitbox()
    {
        BulletCollisionCheck bcheck = GetComponent<BulletCollisionCheck>();
        bcheck.ChangeToNormalRadius();
        bubbleSprite.enabled = false;
    }

    private void HandleShooting()
    {
        if (input.keyInput.shootPress && Time.time > shootCooldown)
            Shoot();
    }

    private void SetPeaShooter()
    {
        if (peaShooter == null)
            return;

        Spawner.Pattern = peaShooter;
    }

    public void Shoot()
    {
        /*if (!canShoot && !debugWeapons)
            return;*/

        Spawner.SpawnBullets(transform.position);
    }

    public void ChangeWeapon(BulletPattern weapon)
    {
        if (weapon == null)
            return;
                
        Spawner.Pattern = weapon;
    }

    public void ChangeBulletHitBehaviour(IBulletHitBehaviour newBulletHitBehaviour)
    {
        BulletHitBehaviour = newBulletHitBehaviour;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        OnPlayerTakeDamage?.Invoke();

        if (Health == 0)
            Die();
    }

    public void Die(bool scorePoints = false)
    {
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        Debug.Log("Doug Dimmadome declares you dimma-dun-dead, son.");

        if (!debugNeverDie)
            OnPlayerDied?.Invoke();
    }

}