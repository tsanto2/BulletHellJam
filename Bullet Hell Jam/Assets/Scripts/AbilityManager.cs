using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{

    [SerializeField]
    private PlayerController pc;

    private DeckManager dm;

    [SerializeField]
    private float weaponDuration = 3.0f;

    private bool isAbsorbing = false;
    private bool isShooting = false;
    private bool isBlocking = false;

    public static event Action OnBlockBulletsCardDeactivated;
    public static event Action OnAbsorbBulletsCardDeactivated;
    public static event Action OnPlayerWeaponCardDeactivated;

    void OnEnable()
    {
        EnergyRegen.OnEnergyRegenCardActivated += ActivateEnergyRegen;
        HpRegen.OnHpRegenCardActivated += ActivateHpRegen;
        PlayerWeapon.OnPlayerWeaponCardActivated += ActivateWeapon;
        AbsorbBullets.OnAbsorbBulletsCardActivated += ActivateAbsorbBullets;
        ClearBullets.OnClearBulletsCardActivated += ActivateClearBullets;
        BlockBullets.OnBlockBulletsCardActivated += ActivateBlockBullets;
        RefreshHand.OnRefreshHandCardActivated += ActivateRefreshHand;
    }

    private void OnDisable()
    {
        EnergyRegen.OnEnergyRegenCardActivated -= ActivateEnergyRegen;
        HpRegen.OnHpRegenCardActivated -= ActivateHpRegen;
        PlayerWeapon.OnPlayerWeaponCardActivated -= ActivateWeapon;
        AbsorbBullets.OnAbsorbBulletsCardActivated -= ActivateAbsorbBullets;
        ClearBullets.OnClearBulletsCardActivated -= ActivateClearBullets;
        RefreshHand.OnRefreshHandCardActivated -= ActivateRefreshHand;
    }

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        dm = FindObjectOfType<DeckManager>();
    }

    void ActivateEnergyRegen(int energyRegenAmount)
    {
        pc.Energy += energyRegenAmount;
    }

    void ActivateHpRegen(int hpRegenAmount)
    {
        pc.Health += hpRegenAmount;
    }

    void ActivateWeapon(BulletPattern bulletPattern, float duration)
    {
        pc.ChangeWeapon(bulletPattern);

        if (!pc.debugWeapons)
        {
            if (!isShooting)
            {
                StartCoroutine(DisablePlayerWeaponAbilityCountdown(duration));
            }
            else
            {
                StopCoroutine(DisablePlayerWeaponAbilityCountdown(0));

                StartCoroutine(DisablePlayerWeaponAbilityCountdown(duration));
            }
        }
    }

    void ActivateAbsorbBullets(float duration)
    {
        pc.ChangeBulletHitBehaviour(new AbsorbBulletBehaviour());

        if (!isAbsorbing)
        {
            StartCoroutine(DisableAbsorbAbilityCountdown(duration));
        }
        else
        {
            StopCoroutine(DisableAbsorbAbilityCountdown(0));

            StartCoroutine(DisableAbsorbAbilityCountdown(duration));
        }

    }

    void ActivateClearBullets()
    {
        ObjectPool.Instance.WipeAllEnemyBullets();
    }

    void ActivateBlockBullets(float duration)
    {
        pc.ChangeBulletHitBehaviour(new BlockBulletBehaviour());

        if (!isBlocking)
        {
            StartCoroutine(DisableBlockAbilityCountdown(duration));
        }
        else
        {
            StopCoroutine(DisableBlockAbilityCountdown(0));

            StartCoroutine(DisableBlockAbilityCountdown(duration));
        }
    }

    void ActivateRefreshHand()
    {
        dm.RenewHand();
    }

    IEnumerator DisablePlayerWeaponAbilityCountdown(float duration)
    {
        pc.CanShoot = true;

        // Might change duration to scale with tier
        yield return new WaitForSeconds(duration);

        pc.CanShoot = false;

        OnPlayerWeaponCardDeactivated?.Invoke();
    }

    IEnumerator DisableAbsorbAbilityCountdown(float duration)
    {
        isAbsorbing = true;

        yield return new WaitForSeconds(duration);

        pc.ChangeBulletHitBehaviour(new DamagePlayerBehaviour(pc));

        isAbsorbing = false;

        OnAbsorbBulletsCardDeactivated?.Invoke();
    }

    IEnumerator DisableBlockAbilityCountdown(float duration)
    {
        isBlocking = true;

        yield return new WaitForSeconds(duration);

        pc.ChangeBulletHitBehaviour(new DamagePlayerBehaviour(pc));

        isBlocking = false;

        OnBlockBulletsCardDeactivated?.Invoke();
    }

}
