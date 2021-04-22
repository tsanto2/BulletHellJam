using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButtonBecauseUnityHatesMe : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip hoverSFX;
    [SerializeField] private AudioClip clickSFX;

    private AudioSource source;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CheckIfRevealed())
            return;

        source.clip = clickSFX;
        source.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CheckIfRevealed())
            return;

        source.clip = hoverSFX;
        source.Play();
    }

    private bool CheckIfRevealed()
    {
        return GetComponent<Image>().color.a > 0.7f;
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
}
