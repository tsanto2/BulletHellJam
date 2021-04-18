using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Cards/BlockBullets")]
public class BlockBullets : Card
{
    [SerializeField]
    private float effectDuration;

    public static event Action<float> OnBlockBulletsCardActivated;
    public override void Activate()
    {
        OnBlockBulletsCardActivated?.Invoke(effectDuration);
    }

}