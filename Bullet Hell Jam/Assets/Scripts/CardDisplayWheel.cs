using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayWheel : MonoBehaviour
{

    [SerializeField]
    private Image displayWheelBG;
    [SerializeField]
    private CardDisplay topCardDisplay;
    [SerializeField]
    private CardDisplay bottomCardDisplay;
    [SerializeField]
    private CardDisplay leftCardDisplay;
    [SerializeField]
    private CardDisplay rightCardDisplay;

    private List<CardDisplay> cardDisplays;

    [SerializeField]
    private DeckManager deckManager;

    private bool isDisplaying = false;

    private void OnEnable()
    {
        DeckManager.OnHandUpdated += UpdateCards;
        GameManager.OnSlowMoStarted += OnSlowMoStarted;
        GameManager.OnSlowMoEnded += OnSlowMoEnded;
    }

    private void OnDisable()
    {
        DeckManager.OnHandUpdated -= UpdateCards;
        GameManager.OnSlowMoStarted -= OnSlowMoStarted;
        GameManager.OnSlowMoEnded -= OnSlowMoEnded;
    }

    private void Start()
    {
        displayWheelBG.enabled = false;

        cardDisplays = new List<CardDisplay>();

        cardDisplays.Add(topCardDisplay);
        cardDisplays.Add(bottomCardDisplay);
        cardDisplays.Add(leftCardDisplay);
        cardDisplays.Add(rightCardDisplay);
    }

    private void OnSlowMoStarted()
    {
        isDisplaying = true;

        displayWheelBG.enabled = true;

        UpdateCards();
    }

    private void OnSlowMoEnded()
    {
        isDisplaying = false;

        displayWheelBG.enabled = false;

        HideAllCardDisplays();
    }

    private void HideAllCardDisplays()
    {
        foreach (CardDisplay cardDisplay in cardDisplays)
        {
            cardDisplay.gameObject.SetActive(false);
        }
    }

    private void UpdateCards()
    {
        if (isDisplaying)
        {
            for (int i = 0; i < deckManager.hand.Count; i++)
            {
                if (deckManager.hand[i] != null)
                {
                    cardDisplays[i].gameObject.SetActive(true);
                    BuildCard(deckManager.hand[i], i);
                }
                else
                    cardDisplays[i].gameObject.SetActive(false);
            }
        }
    }

    private void BuildCard(Card card, int displayIndex)
    {
        CardDisplay cardDisplay = cardDisplays[displayIndex];

        cardDisplay.SetIcon(card.cardIcon);
        cardDisplay.SetBackground(card.cardBorder);
        cardDisplay.SetEnergyCostText(card.energyCost.ToString());
        cardDisplay.SetDescriptionText(card.description);
    }

}
