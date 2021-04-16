﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{

    #region Variables/Fields

    // Gross but just for ease of balance testing
    [SerializeField]
    private int baseDeckBasicHpCardCount, baseDeckBasicEnergyCardCount, baseDeckBasicWeaponCardCount, baseDeckIntermediateHpCardCount, baseDeckIntermediateEnergyCardCount, baseDeckIntermediateWeaponCardCount, baseDeckAdvancedHpCardCount, baseDeckAdvancedEnergyCardCount, baseDeckAdvancedWeaponCardCount;

    [SerializeField]
    private Card[] cardSOs;

    public List<Card> deck;

    public List<Card> Deck { get { return deck; } }

    private List<Card> availableCards;

    private List<Card> discardedCards;

    #endregion


    #region MonoBehaviours
    private void Start()
    {
        CompileBaseDeck();

        availableCards = deck;
    }

    #endregion

    #region Helpers

    // Gnarly
    private void CompileBaseDeck()
    {
        foreach (Card cardSO in cardSOs)
        {
            CardType cType = cardSO.cardType;

            switch (cType)
            {
                case CardType.basicHp:
                    for (int i = 0; i < baseDeckBasicHpCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.basicEnergy:
                    for (int i = 0; i < baseDeckBasicEnergyCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.basicWeapon:
                    for (int i = 0; i < baseDeckBasicWeaponCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.intermediateHp:
                    for (int i = 0; i < baseDeckIntermediateHpCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.intermediateEnergy:
                    for (int i = 0; i < baseDeckIntermediateEnergyCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.intermediateWeapon:
                    for (int i = 0; i < baseDeckIntermediateWeaponCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.advancedHp:
                    for (int i = 0; i < baseDeckAdvancedHpCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.advancedEnergy:
                    for (int i = 0; i < baseDeckAdvancedEnergyCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.advancedWeapon:
                    for (int i = 0; i < baseDeckAdvancedWeaponCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
            }
        }
    }

    #endregion

}
