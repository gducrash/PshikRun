using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public string[] audioNames;

    Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    void Start()
    {
        for (int i = 0; i < audioNames.Length; i++)
        {
            sounds.Add(audioNames[i], audioClips[i]);
        }
    }

    public void StartSound(string name, float volume = 1f, float pitch = 1f)
    {
        // create new audio source
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = sounds[name];
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        // destroy audio source after clip has finished playing
        Destroy(source, source.clip.length / source.pitch);
    }
}
