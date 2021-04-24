using System;
using UnityEngine;

[RequireComponent(typeof(BulletCollisionCheck))]
public class EnemyController : MonoBehaviour, IDamageable, IFireable
{
    public static event Action<int> OnEnemyDeathScore;
    public static event Action<int> OnEnemyDeath;

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
    [SerializeField] private Sound deathSound;
    
    public BulletSpawner Spawner { get; private set; }

    [SerializeField] private bool waitForAwake = true;
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
        GetComponent<BulletCollisionCheck>().enabled = false;
    }

    protected virtual void FixedUpdate()
    {
        if (!onScreen)
            return;

        if (awake || !waitForAwake)
            HandleShooting();
    }

    private void OnBecameVisible()
    {
        onScreen = true;
        Invoke("StartDamage", 0.3f);
    }

    public void WakeUp()
    {
        awake = true;
    }

    private void StartDamage()
    {
        GetComponent<BulletCollisionCheck>().enabled = true;
    }

    public virtual void Die(bool scorePoints = false)
    {
        ObjectPool.Instance.ReturnObject(this.gameObject);
        
        // Increase combo by 1
        
        if (scorePoints)
        {
            OnEnemyDeath?.Invoke(1);
            OnEnemyDeathScore?.Invoke(scoreValue);
            AudioManager.PlaySFX(deathSound, true);
        }
    }

    private void HandleShooting()
    {
        if (Spawner.Pattern == null)
            return;

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
