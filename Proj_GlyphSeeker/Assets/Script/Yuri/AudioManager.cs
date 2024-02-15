using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup MixerMaster, MixerMusic, MixerSFX;

    public Slider masterSlider, musicSlider, sfxSlider;

    public void SetMaster(float sliderValue)
    {
        MixerSFX.audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusic(float sliderValue)
    {
        MixerMusic.audioMixer.SetFloat("Musica", Mathf.Log10(sliderValue) * 20);
    }
    public void SetSFX(float sliderValue)
    {
        MixerSFX.audioMixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }
}
