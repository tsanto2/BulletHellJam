using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Cards/RefreshHand")]
public class RefreshHand : Card
{

    public static event Action OnRefreshHandCardActivated;
    public override void Activate()
    {
        OnRefreshHandCardActivated?.Invoke();
    }

}