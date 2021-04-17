using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Cards/ClearBullets")]
public class ClearBullets : Card
{

    public static event Action OnClearBulletsCardActivated;
    public override void Activate()
    {
        OnClearBulletsCardActivated?.Invoke();
    }

}