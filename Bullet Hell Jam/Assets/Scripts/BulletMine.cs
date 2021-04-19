using UnityEngine;

public class BulletMine : BulletController, IDamageable
{
    [SerializeField, Min(1)] private int healthMax = 2;
    private int health;
    public int Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, healthMax); }
    }

    [SerializeField] private BulletPattern burstWeapon;
    [SerializeField] private Sound deathSound;
    public IBulletHitBehaviour BulletHitBehaviour { get; private set; }


    protected override void OnEnable()
    {
        base.OnEnable();
        Health = healthMax;

        BulletHitBehaviour = new DamagePlayerBehaviour(this);
    }

    public override void Die(bool scorePoints)
    {
        burstWeapon.SpawnBullets(transform.position);
        
        if (scorePoints)
            AudioManager.PlaySFX(deathSound);
            
        pool.ReturnObject(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health == 0)
            Die(true);
    }
}
