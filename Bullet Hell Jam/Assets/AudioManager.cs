using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Don't hurt me
    private static AudioManager Instance;

    [SerializeField] private AudioMixerGroup masterMixer, sfxMixer, bgmMixer;
    [Space]
    private Queue<AudioSource> audioSources = new Queue<AudioSource>();

    private void Awake()
    {
        Instance = this;
    }

    private AudioSource CreateAudioSource()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        return audioSource;
    }

    public static void PlaySFX(Sound sound)
    {
        if (sound == null)
            return;

        AudioSource source;
        if (Instance.audioSources.Count == 0)
            source = Instance.CreateAudioSource();
        else
            source = Instance.audioSources.Dequeue();

        Instance.LoadSound(source, sound);

        if (!sound.loop)
            Instance.StartCoroutine(Instance.Play(source));
        else
        {
            source.Play();
        }
    }

    private void LoadSound(AudioSource source, Sound sound)
    {
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.loop = sound.loop;
        source.outputAudioMixerGroup = sound.output;
    }

    IEnumerator Play(AudioSource source)
    {
        source.Play();

        yield return new WaitForSeconds(source.clip.length);

        source.Stop();
        audioSources.Enqueue(source);
    }
}