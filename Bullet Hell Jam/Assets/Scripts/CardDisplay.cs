using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Image backgroundImage;
    
    [SerializeField]
    private TextMeshProUGUI energyCostText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;
    public void SetIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }

    public void SetBackground(Sprite sprite)
    {
        backgroundImage.sprite = sprite;
    }

    public void SetEnergyCostText(string energyCost)
    {
        energyCostText.text = energyCost;
    }

    public void SetDescriptionText(string description)
    {
        descriptionText.text = description;
    }

}
