using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Cards/Weapon")]
public class PlayerWeapon : Card
{
    [SerializeField]
    private BulletPattern bulletPattern;

    [SerializeField]
    private float effectDuration;

    public static event Action<BulletPattern, float> OnPlayerWeaponCardActivated;

    public override void Activate()
    {
        OnPlayerWeaponCardActivated?.Invoke(bulletPattern, effectDuration);
    }
}
