using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{

    [SerializeField]
    private PlayerController pc;

    void OnEnable()
    {
        EnergyRegen.OnEnergyRegenCardActivated += ActivateEnergyRegen;
        HpRegen.OnHpRegenCardActivated += ActivateHpRegen;
        PlayerWeapon.OnHpRegenCardActivated += ActivateWeapon;
        AbsorbBullets.OnAbsorbBulletsCardActivated += ActivateAbsorbBullets;
    }

    private void OnDisable()
    {
        EnergyRegen.OnEnergyRegenCardActivated -= ActivateEnergyRegen;
        HpRegen.OnHpRegenCardActivated -= ActivateHpRegen;
        PlayerWeapon.OnHpRegenCardActivated -= ActivateWeapon;
        AbsorbBullets.OnAbsorbBulletsCardActivated -= ActivateAbsorbBullets;
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
    }

    void ActivateAbsorbBullets(float duration)
    {
        pc.ChangeBulletHitBehaviour(new AbsorbBulletBehaviour());
    }

}
