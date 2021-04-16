using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{

    [SerializeField]
    private DeckManager deckManager;

    [SerializeField]
    private int maxDrawCount = 4;

    // Change back to private l8r
    public List<Card> hand;

    private void Update()
    {
        // Just for testing :]
        if (Input.GetKeyDown(KeyCode.J))
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
        }
    }

    // Need to account for hand still having cards in it.
    public void DrawHand()
    {
        int drawCount = maxDrawCount - hand.Count;

        for (int i=0; i<drawCount; i++)
        {
            Card drawnCard = deckManager.DrawCard();
            hand.Add(drawnCard);
        }
    }

    // Change this to spend specific card when we are actually spending cards.
    public void SpendCard()
    {
        // Random for testing...
        int spentIndex = Random.Range(0, hand.Count);

        deckManager.AddToDiscardPile(hand[spentIndex]);

        hand.RemoveAt(spentIndex);
    }

    public void DiscardHand()
    {
        for (int i=0; i<hand.Count; i++)
        {
            deckManager.AddToDiscardPile(hand[i]);
        }

        hand.Clear();
    }

}
