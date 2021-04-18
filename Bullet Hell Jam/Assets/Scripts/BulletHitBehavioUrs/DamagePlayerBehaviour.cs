using UnityEngine;

public class DamagePlayerBehaviour : IBulletHitBehaviour
{
    private IDamageable damageable;

    public DamagePlayerBehaviour(IDamageable damageable)
    {
        this.damageable = damageable;
    }

    public void Perform(GameObject bullet)
    {
        if (bullet.TryGetComponent<IDamageable>(out var check))
            check.TakeDamage(1);
        else
            ObjectPool.Instance.ReturnObject(bullet);

        damageable.TakeDamage(1);
    }
}
