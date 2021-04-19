using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/Sound")]
public class Sound : ScriptableObject
{
    public AudioClip clip;
    public AudioMixerGroup output;
    [Space]
    public float volume = 0.2f;
    public float pitch = 1f;
    public bool loop;
}
