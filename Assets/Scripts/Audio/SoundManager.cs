using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }
    
    [SerializeField] private AudioSource _audioSource;
    
    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _loseClip;
    [SerializeField] private AudioClip _matchClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayWinSound()
    {
        if (_winClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_winClip);
        }
    }

    public void PlayLoseSound()
    {
        if (_loseClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_loseClip);
        }
    }
    public void PlayMatchSound()
    {
        if (_matchClip != null && _audioSource != null)
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f); 
            
            _audioSource.PlayOneShot(_matchClip);
            
        }
    }
}