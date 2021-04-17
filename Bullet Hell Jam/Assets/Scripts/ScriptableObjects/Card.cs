using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Card : ScriptableObject {

    // Going to get rid of this later
    public CardType cardType;

    public CardTier cardTier;

    public int energyCost;
    public string description;

    public Sprite cardIcon;
    public Sprite cardBorder;

    public abstract void Activate();

}
