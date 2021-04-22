using UnityEngine;
using System.Collections;

[RequireComponent(typeof(IDamageable)), RequireComponent(typeof(SpriteRenderer))]
public class BulletCollisionCheck : MonoBehaviour
{
    [SerializeField] private LayerMask bulletLayerMask;
    [SerializeField] private float checkRadius;
    [SerializeField] private float normalRadius;
    [SerializeField] private float largeRadius;
    [SerializeField] private bool hitFlash = true;

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

    private void OnEnable()
    {
        PlayerController.OnPlayerDied += StopChecks;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDied -= StopChecks;
    }

    private void FixedUpdate() => CheckForEnemyBullets();

    public void StopChecks() => hitFlash = false;
    public void GainInvincibility(float time) => hitFlash = false;
    public void LoseInvincibility() => hitFlash = true;

    public void ChangeToLargeRadius()
    {
        checkRadius = largeRadius;
    }

    public void ChangeToNormalRadius()
    {
        checkRadius = normalRadius;
    }

    private void CheckForEnemyBullets()
    {
        hit = Physics2D.OverlapCircle(transform.position, checkRadius, bulletLayerMask);
        if (hit)
        {  
            if (hitFlash)
            {
                StartCoroutine(HitFlash());

                if (hitSound != null)
                    AudioManager.PlaySFX(hitSound);
            }

            damageable.BulletHitBehaviour.Perform(hit.gameObject);
        }
    }

    IEnumerator HitFlash()
    {
        spriteRenderer.color = hitFlashColor;

        yield return hitFlashTimer;

        spriteRenderer.color = Color.white;
    }
}
