using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{

    [SerializeField]
    private DeckManager deckManager;

    [SerializeField]
    private int drawCount = 4;

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
    }

    public void DrawHand()
    {
        for (int i=0; i<drawCount; i++)
        {
            hand.Add(deckManager.DrawCard());
        }
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
