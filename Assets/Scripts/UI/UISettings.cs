using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer gameMixer;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private Slider gameVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    private void Start()
    {
        gameVolumeSlider.value = PlayerPrefs.GetFloat("GameVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SetGameVolume(float volume)
    {
        PlayerPrefs.SetFloat("GameVolume", volume);
        gameMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        musicMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }
}
