using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayWheel : MonoBehaviour
{

    [SerializeField]
    private CardDisplay[] cardDisplays;

    [SerializeField]
    private DeckManager deckManager;

    private bool isDisplaying = false;

    private void OnEnable()
    {
        DeckManager.OnHandUpdated += UpdateCards;
    }

    private void OnDisable()
    {
        DeckManager.OnHandUpdated -= UpdateCards;
    }

    private void OnSlowMoStarted()
    {
        isDisplaying = true;
    }

    private void OnSlowMoEnded()
    {
        isDisplaying = false;

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
            for (int i = 0; i < cardDisplays.Length; i++)
            {
                if (i < deckManager.hand.Count)
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
        cardDisplays[displayIndex].SetSprite(card.cardIcon);
    }

}
