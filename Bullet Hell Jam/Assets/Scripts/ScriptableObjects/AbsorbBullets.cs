using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Cards/AbsorbBullets")]
public class AbsorbBullets : Card
{
    [SerializeField]
    private float effectDuration;

    public static event Action<float> OnAbsorbBulletsCardActivated;
    public override void Activate()
    {
        OnAbsorbBulletsCardActivated?.Invoke(effectDuration);
    }

}