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
    }

    private void OnDisable()
    {
        EnergyRegen.OnEnergyRegenCardActivated -= ActivateEnergyRegen;
        HpRegen.OnHpRegenCardActivated -= ActivateHpRegen;
    }

    void ActivateEnergyRegen(int energyRegenAmount)
    {
        Debug.Log("Activating: Regenerating " + energyRegenAmount + " energy.");

        pc.Energy += energyRegenAmount;
    }

    void ActivateHpRegen(int hpRegenAmount)
    {
        Debug.Log("Activating: Regenerating " + hpRegenAmount + " HP.");
    }

}
