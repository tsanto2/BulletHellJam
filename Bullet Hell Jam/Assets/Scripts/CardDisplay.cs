using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer iconSR;

    [SerializeField]
    private SpriteRenderer backgroundSR;
    public void SetIcon(Sprite sprite)
    {
        iconSR.sprite = sprite;
    }

    public void SetBackground(Sprite sprite)
    {
        backgroundSR.sprite = sprite;
    }

}
