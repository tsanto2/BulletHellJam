using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class BulletCollisionCheck : MonoBehaviour
{
    [SerializeField] private LayerMask bulletLayerMask;
    [SerializeField] private float checkRadius;
    [SerializeField] private float normalRadius;
    [SerializeField] private float largeRadius;
    [SerializeField] private Sound hitSound;
    [SerializeField] private bool invincible;

    private IDamageable damageable;
    private Collider2D hit;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    private void FixedUpdate() => CheckForEnemyBullets();

    public void ChangeToLargeRadius()
    {
        checkRadius = largeRadius;
        invincible = true;
    }

    public void ChangeToNormalRadius()
    {
        checkRadius = normalRadius;
        invincible = false;
    }

    private void CheckForEnemyBullets()
    {
        if (invincible)
            return;

        hit = Physics2D.OverlapCircle(transform.position, checkRadius, bulletLayerMask);
        if (hit)
        {
            damageable.BulletHitBehaviour.Perform(hit.gameObject);

            if (hitSound != null)
                AudioManager.PlaySFX(hitSound);
        }
    }
}
