﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{

    public static event Action OnHandUpdated;

    #region Variables/Fields

    // Might be HANDy later for invoking OnHandUpdated
    private enum HandUpdateType
    {
        drawCard = 0,
        discardCard,
        replaceHand,
    }

    // Gross but just for ease of balance testing
    [SerializeField]
    private int baseDeckBasicHpCardCount, baseDeckBasicEnergyCardCount, baseDeckBasicWeaponCardCount, baseDeckIntermediateHpCardCount, baseDeckIntermediateEnergyCardCount, baseDeckIntermediateWeaponCardCount, baseDeckAdvancedHpCardCount, baseDeckAdvancedEnergyCardCount, baseDeckAdvancedWeaponCardCount;

    [SerializeField]
    private Card[] cardSOs;

    [SerializeField]
    private InputController input;

    [SerializeField]
    private PlayerController pc;

    private List<Card> deck;

    public List<Card> Deck { get { return deck; } }

    public List<Card> availableCards;

    public List<Card> AvailableCards { get { return availableCards; } }

    public List<Card> discardedCards;

    public List<Card> DiscardedCards { get { return discardedCards; } }

    [SerializeField]
    private int maxDrawCount = 4;

    // Change back to private l8r
    public List<Card> hand;

    private bool isSlowMoTime = false;

    #endregion

    #region MonoBehaviours

    private void OnEnable()
    {
        GameManager.OnTenSecondsPassed += RenewHand;
        GameManager.OnSlowMoStarted += SlowMoStarted;
        GameManager.OnSlowMoEnded += SlowMoEnded;
    }

    private void OnDisable()
    {
        GameManager.OnTenSecondsPassed -= RenewHand;
        GameManager.OnSlowMoStarted -= SlowMoStarted;
        GameManager.OnSlowMoEnded -= SlowMoEnded;
    }

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        // Just for testing :]
        /*if (Input.GetKeyDown(KeyCode.J))
        {
            DiscardHand();
            DrawHand();
        }
        if (Input.GetKeyDown(KeyCode.G) && hand.Count < maxDrawCount)
        {
            DrawHand();
        }
        if (Input.GetKeyDown(KeyCode.H) && hand.Count > 0)
        {
            SpendCard();
        }*/

        CheckSpendCard();

    }

    #endregion

    #region Events
    private void SlowMoStarted()
    {
        isSlowMoTime = true;
    }

    private void SlowMoEnded()
    {
        isSlowMoTime = false;
    }

    private void RenewHand()
    {
        DiscardHand();
        DrawHand();
    }

    #endregion

    #region Methods

    private void CheckSpendCard()
    {
        if (isSlowMoTime)
        {
            // Dont ask about these numbers and never speak of them again
            if (input.keyInput.bottomFaceButtonPress && hand.Count > 0)
            {
                SpendCard(0);
            }
            if (input.keyInput.topFaceButtonPress && hand.Count > 1)
            {
                SpendCard(1);
            }
            if (input.keyInput.leftFaceButtonPress && hand.Count > 2)
            {
                SpendCard(2);
            }
            if (input.keyInput.rightFaceButtonPress && hand.Count > 3)
            {
                SpendCard(3);
            }
        }
    }

    public Card DrawCard()
    {
        Card drawnCard = null;

        if (availableCards.Count > 0) {
            int randomIndex = Random.Range(0, availableCards.Count);

            drawnCard = availableCards[randomIndex];

            availableCards.RemoveAt(randomIndex);

            OnHandUpdated?.Invoke();
        }
        else
        {
            ShuffleDeck();

            int randomIndex = Random.Range(0, availableCards.Count);

            drawnCard = availableCards[randomIndex];

            availableCards.RemoveAt(randomIndex);

            OnHandUpdated?.Invoke();
        }
        return drawnCard;
    }

    public void AddToDiscardPile(Card discardedCard)
    {
        discardedCards.Add(discardedCard);
    }

    public void ShuffleDeck()
    {
        // Maybe move this out so it's more generic
        availableCards.Clear();

        foreach(Card discardedCard in discardedCards)
        {
            availableCards.Add(discardedCard);
        }

        discardedCards.Clear();

        // Probably doesnt need to actually be shuffled if we make sure we are drawing at random
        for (int i = 0; i < availableCards.Count; i++)
        {
            Card temp = availableCards[i];
            int randomIndex = Random.Range(i, availableCards.Count);
            availableCards[i] = availableCards[randomIndex];
            availableCards[randomIndex] = temp;
        }

        OnHandUpdated?.Invoke();
    }

    public void DrawHand()
    {
        int drawCount = maxDrawCount - hand.Count;

        for (int i = 0; i < drawCount; i++)
        {
            Card drawnCard = DrawCard();
            hand.Add(drawnCard);
        }

        OnHandUpdated?.Invoke();
    }

    public void SpendCard(int spentIndex)
    {
        // Random for testing...
        //int spentIndex = Random.Range(0, hand.Count);

        if (hand[spentIndex] != null && CanActivate(spentIndex))
        {
            pc.Energy -= hand[spentIndex].energyCost;

            hand[spentIndex].Activate();

            AddToDiscardPile(hand[spentIndex]);

            hand[spentIndex] = null;

            OnHandUpdated?.Invoke();
        }
    }

    public void DiscardHand()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i] != null)
                AddToDiscardPile(hand[i]);
        }

        hand.Clear();

        OnHandUpdated?.Invoke();
    }

    #endregion

    #region Helpers

    private bool CanActivate(int cardIndex)
    {

        return hand[cardIndex].energyCost <= pc.Energy;
    }

    #endregion

    #region Initialization

    private void Init()
    {
        deck = new List<Card>();
        discardedCards = new List<Card>();
        hand = new List<Card>();

        CompileBaseDeck();

        availableCards = deck;

        DrawHand();
    }

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
