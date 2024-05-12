using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Singleton used to play sounds anywhere that they're required.
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    public List<Sound> Sounds = new List<Sound>();

    private bool musicCanPlay = true;
    private float fadeDuration = 1.0f;
    private List<string> fadeSounds = new List<string>();
    private Dictionary<string,SoundFadeInfo> fadeMap = new Dictionary<string, SoundFadeInfo>();

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        //Loads in the sounds.
        foreach (Sound sound in Sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.looping;
        }
    }

    void Update()
    {
        FadeUpdate();
    }
    private void FadeUpdate()
    {
        List<string> soundsToStopFading = new List<string>();
        foreach(string fadeSound in fadeSounds)
        {
            fadeMap[fadeSound].timeFading += Time.deltaTime;
            float newVolume = fadeMap[fadeSound].originalVolume * (1 - fadeMap[fadeSound].timeFading / fadeDuration);
            fadeMap[fadeSound].sound.source.volume = newVolume;
            if(fadeMap[fadeSound].timeFading > fadeMap[fadeSound].duration)
            {
                soundsToStopFading.Add(fadeSound);
            }
        }
        foreach(string soundToStopFading in soundsToStopFading)
        {
            AudioManager.Instance.StopSound(soundToStopFading);
            fadeMap.Remove(soundToStopFading);
            fadeSounds.Remove(soundToStopFading);
        }
    }

    //Plays a sound of corresponding name.
    //canPlayMultiple indicates whether the same sound can be played multiple times.
    public void PlaySound(string name, bool canPlayMultiple = false)
    {
        Sound selectedSound = Sounds.Find(s => s.name == name);

        if (selectedSound == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
            return;
        }

        if (!selectedSound.source.isPlaying || canPlayMultiple)
        {
            selectedSound.source.Play();
        }
    }

    public void FadeOutSound(string name, float duration)
    {
        if(fadeMap.ContainsKey(name))
        {
            return;
        }
        fadeSounds.Add(name);
        SoundFadeInfo soundFadeInfo = new SoundFadeInfo();
        soundFadeInfo.timeFading = 0.0f;
        soundFadeInfo.sound = Sounds.Find(s => s.name == name);
        soundFadeInfo.originalVolume = soundFadeInfo.sound.source.volume;
        soundFadeInfo.duration = duration;
        fadeMap.Add(name, soundFadeInfo);
    }

    //Stops the corresponding sound from playing.
    public void StopSound(string name)
    {
        Sound selectedSound = Sounds.Find(s => s.name == name);

        if (selectedSound == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
            return;
        }

        if (selectedSound.source.isPlaying)
        {
            selectedSound.source.Stop();
        }
    }

    //Changes the volume of all sounds.
    public void SetGameVolume(float newVolume)
    {
        foreach (Sound sound in Sounds)
        {
            sound.source.volume = newVolume * sound.volume;
        }
    }

    public void SetMusicVolume(float newVolume)
    {
        foreach (Sound sound in Sounds)
        {
            if(sound.type == Sound.Type.music)
            {
                sound.source.volume = newVolume * sound.volume;
            }
            
        }
    }

    public void SetEffectVolume(float newVolume)
    {
        foreach (Sound sound in Sounds)
        {
            if(sound.type == Sound.Type.sfx)
            {
                sound.source.volume = newVolume * sound.volume;
            }
            
        }
    }

    //Stops all sounds from playing.
    public void StopAll()
    {
        foreach (Sound sound in Sounds)
        {
            sound.source.Stop();
        }
    }

    public void SetMusic(bool isOn)
    {
        musicCanPlay = isOn;
        if (isOn)
        {
            foreach (Sound sound in Sounds)
            {
                if(sound.type == Sound.Type.music)
                {
                    sound.source.volume = sound.volume;
                }
            }
        }
        else
        {
            foreach (Sound sound in Sounds)
            {
                if(sound.type == Sound.Type.music)
                {
                    sound.source.volume = 0.0f;
                }
            }
        }
    }

    private class SoundFadeInfo
    {
        public float timeFading = 0.0f;
        public float originalVolume = 0.0f;
        public Sound sound;
        public float duration = 1.0f;
    }
}
