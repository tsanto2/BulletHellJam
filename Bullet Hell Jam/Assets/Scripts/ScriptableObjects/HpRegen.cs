using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Cards/HpRegen")]
public class HpRegen : Card
{
    [SerializeField]
    private int hpRegenAmount;

    public static event Action<int> OnHpRegenCardActivated;
    public override void Activate()
    {
        OnHpRegenCardActivated?.Invoke(hpRegenAmount);
    }

}
