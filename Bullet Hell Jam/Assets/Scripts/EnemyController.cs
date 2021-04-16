using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable, IFireable
{
    public static event Action<int> OnEnemyDeathScore;
    public static event Action OnEnemyDeath;

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

    [SerializeField] private int scoreValue;
    
    [SerializeField] private BulletPattern weapon;
    [SerializeField] private LayerMask playerBulletLayerMask;
    private Collider2D hit;

    private ObjectPool pool;
    private SpriteRenderer spriteRenderer;
    private float scrollSpeed;

    private bool awake;
    private bool onScreen;

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
        onScreen = false;
    }

    private void FixedUpdate()
    {
        if (onScreen)
            CheckForPlayerBullets();

        if (awake)
            HandleShooting();
    }

    private void OnBecameVisible()
    {
        onScreen = true;
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

        OnEnemyDeath?.Invoke();
        OnEnemyDeathScore?.Invoke(scoreValue);
    }

    private void HandleShooting()
    {
        if (Time.time > shootCooldown)
            Shoot(weapon);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (health == 0)
            Die();
    }

    public void Shoot(BulletPattern weapon)
    {
        weapon.SpawnBullets(transform.position, this);
    }
}
