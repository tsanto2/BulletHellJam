using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    private InputController ic;
    private SceneTransitionFader fader;

    [SerializeField]
    private AudioSource wavesAudio;

    //[SerializeField]
    //private TextMeshProUGUI title;

    [SerializeField]
    private Image title;

    [SerializeField]
    private List<Image> buttonImages;
    [SerializeField]
    private List<TextMeshProUGUI> buttonTexts;

    [SerializeField]
    private float titleFadeInDuration = 2f;
    [SerializeField]
    private float titleTranslationSpeed = 2f;
    [SerializeField]
    private float titleDestinationY = 2f;

    [SerializeField]
    private float buttonFadeInDuration = 2f;

    [SerializeField]
    private float sceneTransitionFadeDuration = 2f;

    private bool transitioningToNewScene = false;

    public bool debugControlsScene = false;
    
    [SerializeField] private Button playButton;

    private void Start()
    {
        ic = FindObjectOfType<InputController>();

        fader = FindObjectOfType<SceneTransitionFader>();

        //wavesAudio.Play();

        StartCoroutine(TitleFadeIn());
    }

    private void OnEnable()
    {
        
    }

    public void PlayButtonPressed()
    {
        Debug.Log("Play button pressed.");

        if (!PlayerPrefs.HasKey("HasPlayedBefore") || (PlayerPrefs.GetInt("HasPlayedBefore") != 1) || debugControlsScene)
        {
            PlayerPrefs.SetInt("HasPlayedBefore", 1);
            StartCoroutine(TimedSceneTransition("GameplayControlsScene"));
        }
        else if (!transitioningToNewScene)
            StartCoroutine(TimedSceneTransition("MatTest"));
    }

    public void ControlsSceneButtonPressed()
    {
        TransitionToScene("MainMenuControlsScene");
    }

    private void TransitionToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator TitleFadeIn()
    {
        float elapsedTime = 0;
        float startValue = 0;
        float endValue = 1;

        while (elapsedTime < titleFadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / titleFadeInDuration);
            title.color = new Color(title.color.r, title.color.g, title.color.b, newAlpha);
            yield return null;
        }

        StartCoroutine(TranslateTitleUpwards());
    }

    public IEnumerator TranslateTitleUpwards()
    {
        Transform titleTransform = title.gameObject.transform;
        while (titleTransform.position.y < titleDestinationY)
        {
            titleTransform.Translate(Vector2.up * titleTranslationSpeed * Time.deltaTime);

            yield return null;
        }

        StartCoroutine(MenuButtonFadeIn());
    }

    public IEnumerator MenuButtonFadeIn()
    {
        float elapsedTime = 0;
        float startValue = 0;
        float endValue = 1;

        while (elapsedTime < buttonFadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / buttonFadeInDuration);

            foreach (Image buttonImage in buttonImages)
            {
                buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, newAlpha);
            }

            foreach (TextMeshProUGUI buttonText in buttonTexts)
            {
                buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, newAlpha);
            }

            yield return null;
        }
    }

    IEnumerator TimedSceneTransition(string sceneName = null)
    {
        transitioningToNewScene = true;

        Image faderImage = fader.FaderImage;

        float elapsedTime = 0;
        float startValue = faderImage.color.a;
        float endValue = 1;

        while (elapsedTime < sceneTransitionFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / sceneTransitionFadeDuration);
            faderImage.color = new Color(faderImage.color.r, faderImage.color.g, faderImage.color.b, newAlpha);
            yield return null;
        }

        TransitionToScene(sceneName);
    }

}
