using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Don't hurt me
    private static AudioManager Instance;

    [SerializeField]
    private MixerGroupVolume masterMixer, bgmMixer, sfxMixer;
    [Space]
    [SerializeField] private Sound bgm;
    [SerializeField] private float minLowpassFrequency;

    private Queue<AudioSource> audioSources = new Queue<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            LoadMixerGroupVolume(masterMixer.mixerGroup, masterMixer.volumeString, masterMixer.defaultVolume);
            LoadMixerGroupVolume(bgmMixer.mixerGroup, bgmMixer.volumeString, bgmMixer.defaultVolume);
            LoadMixerGroupVolume(sfxMixer.mixerGroup, sfxMixer.volumeString, sfxMixer.defaultVolume);
        }

        Instance = this;
    }

    private void Start()
    {
        //PlaySFX(bgm);
    }

    private static void LoadMixerGroupVolume(AudioMixerGroup group, string volumeString, float defaultVolume)
    {
        float volume = PlayerPrefs.GetFloat(volumeString, 1f);
        group.audioMixer.SetFloat(volumeString, Mathf.Log10(volume) * 30f);
    }

    private static void UpdateMixerGroupVolume(AudioMixerGroup group, string volumeString, float volume)
    {
        PlayerPrefs.SetFloat(volumeString, volume);
        group.audioMixer.SetFloat(volumeString, Mathf.Log10(volume) * 30f);
    }

    public static void UpdateBGMLowPassFilter()
    {
        float lowpass = (Time.timeScale * (5000f - Instance.minLowpassFrequency));
        Instance.bgmMixer.mixerGroup.audioMixer.SetFloat("BgmLowpassCutoff", Instance.minLowpassFrequency + lowpass);
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

    [Serializable]
    private class MixerGroupVolume
    {
        public AudioMixerGroup mixerGroup;
        public string volumeString;
        public float defaultVolume = 1f;
    }
}