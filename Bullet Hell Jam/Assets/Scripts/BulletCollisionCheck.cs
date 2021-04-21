using UnityEngine;
using System.Collections;

[RequireComponent(typeof(IDamageable)), RequireComponent(typeof(SpriteRenderer))]
public class BulletCollisionCheck : MonoBehaviour
{
    [SerializeField] private LayerMask bulletLayerMask;
    [SerializeField] private float checkRadius;
    [SerializeField] private float normalRadius;
    [SerializeField] private float largeRadius;
    [SerializeField] private bool invincible;

    [Header("Hit FX")]
    [SerializeField] private Sound hitSound;
    [SerializeField] private Color hitFlashColor;
    [SerializeField] private float hitFlashDuration;
    private WaitForSeconds hitFlashTimer;
    private Coroutine hitFlashCoroutine;

    private IDamageable damageable;
    private Collider2D hit;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        hitFlashTimer = new WaitForSeconds(hitFlashDuration);
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
            StartCoroutine(HitFlash());
            damageable.BulletHitBehaviour.Perform(hit.gameObject);

            if (hitSound != null)
                AudioManager.PlaySFX(hitSound);
        }
    }

    IEnumerator HitFlash()
    {
        spriteRenderer.color = hitFlashColor;

        yield return hitFlashTimer;

        spriteRenderer.color = Color.white;
    }
}
