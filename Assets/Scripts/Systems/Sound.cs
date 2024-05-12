using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Used to contain the information required for playing a sound with the AudioManager.
/// </summary>
[System.Serializable]
public class Sound
{
    public enum Type
    {
        sfx, music
    }

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Range(0.1f, 3.0f)]
    public float pitch = 1.0f;

    public bool looping;

    [HideInInspector]
    public AudioSource source;

    public Type type;
}
