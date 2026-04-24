using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.Menu
{
    public class VolumeSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _musicSlider;

        private const string MUSIC_VOLUME_KEY = "MusicVolume";

        private void Start()
        {
            if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
            {
                LoadVolume();
            }
            else
            {
                SetMusicVolume();
            }
        }
        
        private void SetMusicVolume()
        {
            float volume = _musicSlider.value;
            
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
            
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }

        private void LoadVolume()
        {
            _musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
            SetMusicVolume();
        }
    }
}