using UnityEngine;
using UnityEngine.Audio;

namespace UI.Menu
{
    public class AudioManager : MonoBehaviour
    {

       private static AudioManager Instance;

        [SerializeField] private AudioMixer _audioMixer;
        private const string MUSIC_VOLUME_KEY = "MusicVolume";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); 
            }
            else
            {

                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
            {
                float volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
                _audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
            }
        }
    }
}