using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayWheel : MonoBehaviour
{

    [SerializeField]
    private CardDisplay[] cardDisplays;

    [SerializeField]
    private DeckManager deckManager;

    private void OnEnable()
    {
        DeckManager.OnHandUpdated += UpdateCards;
    }

    private void OnDisable()
    {
        
    }

    private void UpdateCards()
    {
        for (int i=0; i<cardDisplays.Length; i++)
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

    private void BuildCard(Card card, int displayIndex)
    {
        cardDisplays[displayIndex].SetSprite(card.cardIcon);
    }

}
