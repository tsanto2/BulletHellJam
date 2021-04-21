using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDAbilityIconManager : MonoBehaviour
{

    //[SerializeField]
    //private GameObject weaponIconObject, absorbIconObject, blockIconObject;

    [SerializeField]
    private Image weaponIconImage, absorbIconImage, blockIconImage;

    [SerializeField]
    private Image weaponIconDurationImage, absorbIconDurationImage, blockIconDurationImage;

    private float weaponDurationRemaining, absorbDurationRemaining, blockDurationRemaining;
    private float weaponDurationTotal, absorbDurationTotal, blockDurationTotal;

    /*[SerializeField]
    private Sprite weaponIcon, absorbIcon, blockIcon;*/

    private enum TimedAbilityType
    {
        weapon = 0,
        absorb,
        block,
    }

    private void OnEnable()
    {
        PlayerWeapon.OnPlayerWeaponCardActivated += WeaponActivated;
        AbsorbBullets.OnAbsorbBulletsCardActivated += AbsorbActivated;
        BlockBullets.OnBlockBulletsCardActivated += BlockActivated;

        AbilityManager.OnPlayerWeaponCardDeactivated += WeaponDeactivated;
        AbilityManager.OnAbsorbBulletsCardDeactivated += AbsorbDeactivated;
        AbilityManager.OnBlockBulletsCardDeactivated += BlockDeactivated;
    }

    private void OnDisable()
    {
        PlayerWeapon.OnPlayerWeaponCardActivated -= WeaponActivated;
        AbsorbBullets.OnAbsorbBulletsCardActivated -= AbsorbActivated;
        BlockBullets.OnBlockBulletsCardActivated -= BlockActivated;

        AbilityManager.OnPlayerWeaponCardDeactivated -= WeaponDeactivated;
        AbilityManager.OnAbsorbBulletsCardDeactivated -= AbsorbDeactivated;
        AbilityManager.OnBlockBulletsCardDeactivated -= BlockDeactivated;
    }

    private void FixedUpdate()
    {
        UpdateIconDuration();
    }

    private void WeaponActivated(BulletPattern bulletPattern, float duration)
    {
        weaponDurationRemaining = duration;
        weaponDurationTotal = duration;
        ManageIcons(TimedAbilityType.weapon, true);
    }

    private void AbsorbActivated(float duration)
    {
        absorbDurationRemaining = duration;
        absorbDurationTotal = duration;
        ManageIcons(TimedAbilityType.absorb, true);
    }

    private void BlockActivated(float duration)
    {
        blockDurationRemaining = duration;
        blockDurationTotal = duration;
        ManageIcons(TimedAbilityType.block, true);
    }

    private void WeaponDeactivated()
    {
        ManageIcons(TimedAbilityType.weapon, false);
    }

    private void AbsorbDeactivated()
    {
        ManageIcons(TimedAbilityType.absorb, false);
    }

    private void BlockDeactivated()
    {
        ManageIcons(TimedAbilityType.block, false);
    }

    private void ManageIcons(TimedAbilityType timedAbilityType, bool shouldActivateIcon)
    {
        switch (timedAbilityType)
        {
            case TimedAbilityType.weapon:
                weaponIconImage.enabled = shouldActivateIcon;
                weaponIconDurationImage.enabled = shouldActivateIcon;
                break;
            case TimedAbilityType.absorb:
                absorbIconImage.enabled = shouldActivateIcon;
                absorbIconDurationImage.enabled = shouldActivateIcon;
                break;
            case TimedAbilityType.block:
                blockIconImage.enabled = shouldActivateIcon;
                blockIconDurationImage.enabled = shouldActivateIcon;
                break;

        }

        UpdateIconDuration();
    }

    private void UpdateIconDuration()
    {
        if (weaponIconDurationImage.enabled)
        {
            weaponIconDurationImage.fillAmount = weaponDurationRemaining / weaponDurationTotal;
            weaponDurationRemaining -= Time.fixedDeltaTime;
        }

        if (absorbIconDurationImage.enabled)
        {
            absorbIconDurationImage.fillAmount = absorbDurationRemaining / absorbDurationTotal;
            absorbDurationRemaining -= Time.fixedDeltaTime;
        }

        if (blockIconDurationImage.enabled)
        {
            blockIconDurationImage.fillAmount = blockDurationRemaining / blockDurationTotal;
            blockDurationRemaining -= Time.fixedDeltaTime;
        }
    }
}
