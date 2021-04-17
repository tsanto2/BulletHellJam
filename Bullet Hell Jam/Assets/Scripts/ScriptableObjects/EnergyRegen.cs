using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Cards/EnergyRegen")]
public class EnergyRegen : Card
{
    [SerializeField]
    private int energyRegenAmount;

    public static event Action<int> OnEnergyRegenCardActivated;
    public override void Activate()
    {
        OnEnergyRegenCardActivated?.Invoke(energyRegenAmount);
    }

}
