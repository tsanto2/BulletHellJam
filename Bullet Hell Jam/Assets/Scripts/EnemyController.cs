using System;
using UnityEngine;

[RequireComponent(typeof(BulletCollisionCheck))]
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

    public IBulletHitBehaviour BulletHitBehaviour { get; private set; }

    [SerializeField] private int scoreValue;
    
    public BulletSpawner Spawner { get; private set; }
    
    [SerializeField] private LayerMask playerBulletLayerMask;
    private Collider2D hit;

    private float scrollSpeed;

    protected bool awake;
    private bool onScreen;

    protected virtual void Awake()
    {
        BulletHitBehaviour = new DamagePlayerBehaviour(this);
        Spawner = GetComponent<BulletSpawner>();
    }

    private void OnEnable()
    {
        health = healthMax;
        awake = false;
        onScreen = false;  
    }

    protected virtual void FixedUpdate()
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
            ObjectPool.Instance.ReturnObject(hit.gameObject);
            TakeDamage(1);
        }
    }

    public virtual void Die(bool scorePoints = false)
    {
        ObjectPool.Instance.ReturnObject(this.gameObject);

        OnEnemyDeath?.Invoke();
        
        if (scorePoints)
            OnEnemyDeathScore?.Invoke(scoreValue);
    }

    private void HandleShooting()
    {
        if (Time.time > shootCooldown)
            Shoot();
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (health == 0)
            Die(true);
    }

    public void Shoot()
    {
        Spawner.SpawnBullets(transform.position);
        ShootCooldown = Spawner.Pattern.shootDelay;
    }
}
