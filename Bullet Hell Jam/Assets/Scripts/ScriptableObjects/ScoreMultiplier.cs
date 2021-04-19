using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Cards/ScoreMultiplier")]
public class ScoreMultiplier : Card
{
    [SerializeField]
    private int multiplierAmount;

    public static event Action<int> OnScoreMultiplierActivated;
    public override void Activate()
    {
        OnScoreMultiplierActivated?.Invoke(multiplierAmount);
    }

}
