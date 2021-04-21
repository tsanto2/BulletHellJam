using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathScreenController : MonoBehaviour
{

    [SerializeField]
    private InputController ic;

    [SerializeField]
    private TextMeshProUGUI youDiedText;

    [SerializeField]
    private AudioSource youDiedSFX;

    [SerializeField]
    private float finalFontSize;

    [SerializeField]
    private float textFadeInTime;

    private bool fadingIn = false;

    private void Start()
    {
        ic = FindObjectOfType<InputController>();

        youDiedSFX.Play();

        StartCoroutine(EnlargeAndFadeInText());
    }

    private void Update()
    {
        if (ic.keyInput.topFaceButtonPress && !fadingIn)
        {
        }
    }

    private IEnumerator EnlargeAndFadeInText()
    {
        fadingIn = true;

        float elapsedTime = 0;
        float startSizeValue = youDiedText.fontSize;

        float startColorValue = youDiedText.color.a;
        float endValue = 1;

        while (elapsedTime < textFadeInTime)
        {
            elapsedTime += Time.deltaTime;
            float newSize = Mathf.Lerp(startSizeValue, finalFontSize, elapsedTime / textFadeInTime);
            youDiedText.fontSize = newSize;

            float newAlpha = Mathf.Lerp(startColorValue, endValue, elapsedTime / textFadeInTime);
            youDiedText.color = new Color(youDiedText.color.r, youDiedText.color.g, youDiedText.color.b, newAlpha);

            yield return null;
        }

        fadingIn = false;

        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("MainMenu");
    }

}
