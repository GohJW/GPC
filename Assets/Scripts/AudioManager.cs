using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public AudioMixerGroup MasterVolume;
    public bool Stage2Played;
    public bool Stage3Played;
    public bool Stage4Played;
    public bool Stage5Played;
    public bool Stage6Played;
    public bool Stage7Played;
    public bool Stage8Played;
    public AudioMixer AudioMixer;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source =  gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = MasterVolume;

        }
    }
    private void Start()
    {
        Play("Theme");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
    public void ChangeVolume(string name, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = value;
    }

    //public void SetVolume(float slidervalue)
    //{
    //    Slider.
    //    AudioMixer.SetFloat("Master", Mathf.Log10(slidervalue) * 20);
    //}
}
