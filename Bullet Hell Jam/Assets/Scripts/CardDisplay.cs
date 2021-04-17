using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer sr;
    public void SetSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }

}
