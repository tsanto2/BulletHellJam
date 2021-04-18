using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class BulletCollisionCheck : MonoBehaviour
{
    [SerializeField] private LayerMask bulletLayerMask;
    [SerializeField] private float checkRadius;

    private IDamageable damageable;
    private Collider2D hit;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    private void FixedUpdate() => CheckForEnemyBullets();

    private void CheckForEnemyBullets()
    {
        hit = Physics2D.OverlapCircle(transform.position, checkRadius, bulletLayerMask);
        Debug.Log(hit);
        if (hit)
        {
            Debug.Log($"Damageable: {damageable}");
            Debug.Log($"Hit Behaviour: {damageable.BulletHitBehaviour}");
            damageable.BulletHitBehaviour.Perform(hit.gameObject);
        }
    }
}
