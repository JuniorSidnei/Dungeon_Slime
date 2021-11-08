using System.Collections;
using GameToBeNamed.Utils;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    #region Fields
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private float musicVolume = 1;
    private bool firstMusicSourceIsActive;
    #endregion

    private void Awake()  {
        DontDestroyOnLoad(this.gameObject);
        
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource2 = gameObject.AddComponent<AudioSource>();
        
        musicSource.loop = true;
        musicSource2.loop = true;
    }

    public void PlayMusic(AudioClip musicClip)  {
        musicSource.clip = musicClip;
        musicSource.Play();
    }
    
    public void PlayMusicWithFade(AudioClip musicClip, float transitionTime = 1.0f)  {
        AudioSource activeSource = (firstMusicSourceIsActive) ? musicSource : musicSource2;

        StartCoroutine(UpdateMusicWithFade(activeSource, musicClip, transitionTime));
    }
    
    public void PlayMusicWithCrossFade(AudioClip musicClip, float transitionTime = 1.0f)  {
        AudioSource activeSource = (firstMusicSourceIsActive) ? musicSource : musicSource2;
        AudioSource newSource = (firstMusicSourceIsActive) ? musicSource2 : musicSource;
        
        firstMusicSourceIsActive = !firstMusicSourceIsActive;
        
        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, musicClip, transitionTime));
    }
    
    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip music, float transitionTime)  {
        if(!activeSource.isPlaying)
            activeSource.Play();

        var t = 0.0f;
        
        for (t = 0.0f; t <= transitionTime; t += Time.deltaTime)  {
            activeSource.volume = (musicVolume - ((t/ transitionTime) * musicVolume));
            yield return null;
        }

        
        activeSource.Stop();
        activeSource.clip = music;
        activeSource.Play();

        for (t = 0.0f; t <= transitionTime; t += Time.deltaTime) {
            activeSource.volume = (t / transitionTime) * musicVolume;
            yield return null;
        }
        
        activeSource.volume = musicVolume;
    }
    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, AudioClip music, float transitionTime)  {
        if (!original.isPlaying)
            original.Play();

        newSource.Stop();
        newSource.clip = music;
        newSource.Play();

        var t = 0.0f;

        for (t = 0.0f; t <= transitionTime; t += Time.deltaTime)  {
            original.volume = (musicVolume - ((t / transitionTime) * musicVolume));
            newSource.volume = (t / transitionTime) * musicVolume;
            yield return null;
        }
        
        original.volume = 0;
        newSource.volume = musicVolume;

        original.Stop();
    }

    public void PlaySFX(AudioClip clip, float volume  = 0.5f, bool loop = false) {
        var newSource = gameObject.AddComponent<AudioSource>();
        newSource.PlayOneShot(clip);
        newSource.volume = volume;
        newSource.loop = loop;

        if (loop) return;
        
        StartCoroutine(DestroySourceAfterFinished(newSource, clip.length));
    }
    
    public void SetMusicVolume(float volume)  {
        musicVolume = musicSource.volume = volume;
        musicSource2.volume = volume;
    }
    
    private IEnumerator DestroySourceAfterFinished(AudioSource source, float clipLength) {
        var  t = Time.realtimeSinceStartup;

        do {
            yield return null;
        } while (Time.realtimeSinceStartup - t < clipLength);
        
        Destroy(source);
    }
}