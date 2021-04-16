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



    public void DrawHand()
    {
        int[] drawnIndices = new int[4];

        for (int i=0; i<drawCount; i++)
        {

        }
    }

}
