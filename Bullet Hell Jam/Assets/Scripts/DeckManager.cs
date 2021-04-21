using System;
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
    private int baseDeckBasicHpCardCount,
                baseDeckIntermediateHpCardCount,
                baseDeckAdvancedHpCardCount,
                baseDeckBasicEnergyCardCount,
                baseDeckIntermediateEnergyCardCount,
                baseDeckAdvancedEnergyCardCount,
                baseDeckBasicWeaponCardCount,
                baseDeckIntermediateWeaponCardCount,
                baseDeckAdvancedWeaponCardCount,
                baseDeckBasicBlockCardCount,
                baseDeckIntermediateBlockCardCount,
                baseDeckAdvancedBlockCardCount,
                baseDeckBasicAbsorbCardCount,
                baseDeckIntermediateAbsorbCardCount,
                baseDeckAdvancedAbsorbCardCount,
                baseDeckBasicRefreshHandCardCount,
                baseDeckBasicClearBullets,
                baseDeckBasicScoreMultiplier,
                baseDeckIntermediateScoreMultiplier,
                baseDeckAdvancedScoreMultiplier;

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

    [SerializeField]
    private Sound activateCardSound, drawHandSound, activateNoEnergySound;

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

    public void RenewHand()
    {
        AudioManager.PlaySFX(drawHandSound);

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
            // Okay, will do.
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
            Card spentCard = hand[spentIndex];

            pc.Energy -= spentCard.energyCost;

            AddToDiscardPile(hand[spentIndex]);

            hand[spentIndex] = null;

            spentCard.Activate();

            OnHandUpdated?.Invoke();

            AudioManager.PlaySFX(activateCardSound);
        }
        else
            AudioManager.PlaySFX(activateNoEnergySound);
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
                case CardType.basicAbsorb:
                    for (int i = 0; i < baseDeckBasicAbsorbCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.basicClearBullets:
                    for (int i = 0; i < baseDeckBasicClearBullets; i++)
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
                case CardType.intermediateAbsorb:
                    for (int i = 0; i < baseDeckIntermediateAbsorbCardCount; i++)
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
                case CardType.advancedAbsorb:
                    for (int i = 0; i < baseDeckAdvancedAbsorbCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.basicBlock:
                    for (int i = 0; i < baseDeckBasicBlockCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.intermediateBlock:
                    for (int i = 0; i < baseDeckIntermediateBlockCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.advancedBlock:
                    for (int i = 0; i < baseDeckAdvancedBlockCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.basicRefreshHand:
                    for (int i = 0; i < baseDeckBasicRefreshHandCardCount; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.basicScoreMultiplier:
                    for (int i = 0; i < baseDeckBasicScoreMultiplier; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.intermediateScoreMultiplier:
                    for (int i = 0; i < baseDeckIntermediateScoreMultiplier; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
                case CardType.advancedScoreMultiplier:
                    for (int i = 0; i < baseDeckAdvancedScoreMultiplier; i++)
                    {
                        deck.Add(cardSO);
                    }
                    break;
            }
        }
    }

    #endregion

}
