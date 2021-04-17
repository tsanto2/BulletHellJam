using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Cards/Weapon")]
public class PlayerWeapon : Card
{
    [SerializeField]
    private BulletPattern bulletPattern;

    public static event Action<BulletPattern> OnHpRegenCardActivated;

    public override void Activate()
    {
        OnHpRegenCardActivated?.Invoke(bulletPattern);
    }
}
