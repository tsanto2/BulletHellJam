public interface IDamageable
{
    int Health { get; }
    IBulletHitBehaviour BulletHitBehaviour { get; }

    void TakeDamage(int damage);
    void Die(bool scorePoints);
}
