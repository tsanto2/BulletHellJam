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

    public float ShootCooldown { get; set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        Health = healthMax;
    }

    public override void Die(bool scorePoints)
    {
        burstWeapon.SpawnBullets(transform.position);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health == 0)
            Die(true);
    }
}
