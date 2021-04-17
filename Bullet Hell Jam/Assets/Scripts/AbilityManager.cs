using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{

    [SerializeField]
    private PlayerController pc;

    [SerializeField]
    private float weaponDuration = 3.0f;

    private bool isAbsorbing = false;
    private bool isShooting = false;

    void OnEnable()
    {
        EnergyRegen.OnEnergyRegenCardActivated += ActivateEnergyRegen;
        HpRegen.OnHpRegenCardActivated += ActivateHpRegen;
        PlayerWeapon.OnHpRegenCardActivated += ActivateWeapon;
        AbsorbBullets.OnAbsorbBulletsCardActivated += ActivateAbsorbBullets;
        ClearBullets.OnClearBulletsCardActivated += ActivateClearBullets;
    }

    private void OnDisable()
    {
        EnergyRegen.OnEnergyRegenCardActivated -= ActivateEnergyRegen;
        HpRegen.OnHpRegenCardActivated -= ActivateHpRegen;
        PlayerWeapon.OnHpRegenCardActivated -= ActivateWeapon;
        AbsorbBullets.OnAbsorbBulletsCardActivated -= ActivateAbsorbBullets;
        ClearBullets.OnClearBulletsCardActivated += ActivateClearBullets;
    }

    void ActivateEnergyRegen(int energyRegenAmount)
    {
        Debug.Log("Activating: Regenerating " + energyRegenAmount + " energy.");

        pc.Energy += energyRegenAmount;
    }

    void ActivateHpRegen(int hpRegenAmount)
    {
        Debug.Log("Activating: Regenerating " + hpRegenAmount + " HP.");

        pc.Health += hpRegenAmount;
    }

    void ActivateWeapon(BulletPattern bulletPattern)
    {
        pc.ChangeWeapon(bulletPattern);

        if (!pc.debugWeapons)
        {
            if (!isShooting)
            {
                StartCoroutine(DisablePlayerWeaponAbilityCountdown());
            }
            else
            {
                StopCoroutine(DisablePlayerWeaponAbilityCountdown());

                StartCoroutine(DisablePlayerWeaponAbilityCountdown());
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

    IEnumerator DisablePlayerWeaponAbilityCountdown()
    {
        pc.CanShoot = true;

        // Might change duration to scale with tier
        yield return new WaitForSeconds(weaponDuration);

        pc.CanShoot = false;
    }

    IEnumerator DisableAbsorbAbilityCountdown(float duration)
    {
        isAbsorbing = true;

        yield return new WaitForSeconds(duration);

        pc.ChangeBulletHitBehaviour(new DamagePlayerBehaviour());

        isAbsorbing = false;
    }

}
