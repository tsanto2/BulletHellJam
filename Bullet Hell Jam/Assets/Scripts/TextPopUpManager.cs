using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopUpManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI centerTextPopUp;

    private Coroutine trackedCardSelectionPressedTimer = null;

    [SerializeField]
    private float cardSelectionPopUpTimerMax = 10.0f;
    private float cardSelectionPopUpTimer = 0f;

    [SerializeField]
    private string cardSelectionPopUpText;

    private void OnEnable()
    {
        GameManager.OnSlowMoStarted += UpdateCardSelectionPressed;
        GameManager.OnSlowMoEnded += UpdateCardSelectionReleased;
    }

    private void OnDisable()
    {
        GameManager.OnSlowMoStarted -= UpdateCardSelectionPressed;
        GameManager.OnSlowMoEnded -= UpdateCardSelectionReleased;
    }

    private void Start()
    {
        trackedCardSelectionPressedTimer = StartCoroutine(CardSelectionPressedTimer());
    }

    private void UpdateCardSelectionPressed()
    {
        StopCoroutine(trackedCardSelectionPressedTimer);
        cardSelectionPopUpTimer = 0;
        HideCenterText();
    }

    private void UpdateCardSelectionReleased()
    {
        trackedCardSelectionPressedTimer = StartCoroutine(CardSelectionPressedTimer());
    }


    private void DisplayCenterText(string text)
    {
        centerTextPopUp.text = text;
        centerTextPopUp.enabled = true;
    }

    private void HideCenterText()
    {
        centerTextPopUp.enabled = false;
    }

    private IEnumerator CardSelectionPressedTimer()
    {
        yield return new WaitForSeconds(0.1f);
        cardSelectionPopUpTimer += 0.1f;

        if (cardSelectionPopUpTimer >= cardSelectionPopUpTimerMax)
        {
            cardSelectionPopUpTimer = 0;
            DisplayCenterText(cardSelectionPopUpText);
            yield break;
        }

        trackedCardSelectionPressedTimer = StartCoroutine(CardSelectionPressedTimer());
    }

}
