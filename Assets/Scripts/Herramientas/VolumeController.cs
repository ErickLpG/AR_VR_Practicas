using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource buttonSoundSource;

    public Slider musicSlider;
    public Slider buttonSlider;

    public GameObject slidersContainer;

    void Start()
    {
        musicSlider.value = musicSource.volume;
        buttonSlider.value = 0.2f;

        buttonSoundSource.volume = 0.2f;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        buttonSlider.onValueChanged.AddListener(SetButtonVolume);

        slidersContainer.SetActive(false);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetButtonVolume(float volume)
    {
        buttonSoundSource.volume = volume;
    }

    public void hide_Show_Sliders()
    {
        slidersContainer.SetActive(!slidersContainer.activeSelf);
    }
}