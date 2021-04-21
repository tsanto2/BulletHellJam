using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionFader : MonoBehaviour
{

    [SerializeField]
    private Image faderImage;

    [SerializeField]
    private bool isDeathFader;

    public bool IsDeathFader { get { return isDeathFader; } }

    public Image FaderImage { get { return faderImage; } }

}
